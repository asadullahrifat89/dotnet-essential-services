using IdentityCore.Contracts.Declarations.Commands;
using IdentityCore.Contracts.Declarations.Repositories;
using IdentityCore.Contracts.Declarations.Services;
using IdentityCore.Models.Entities;
using IdentityCore.Models.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityCore.Contracts.Implementations.Repositories
{
    public class AuthTokenRepository : IAuthTokenRepository
    {
        #region Fields

        private readonly IMongoDbService _mongoDbService;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IClaimPermissionRepository _claimPermissionRepository;
        private readonly IConfiguration _configuration;

        #endregion

        #region Ctor

        public AuthTokenRepository(
           IMongoDbService mongoDbService,
           IUserRepository userRepository,
           IConfiguration configuration,
           IRoleRepository roleRepository,
           IClaimPermissionRepository claimPermissionRepository)
        {
            _mongoDbService = mongoDbService;
            _userRepository = userRepository;
            _configuration = configuration;
            _roleRepository = roleRepository;
            _claimPermissionRepository = claimPermissionRepository;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> Authenticate(AuthenticateCommand command)
        {
            var user = await _userRepository.GetUser(userEmail: command.Email, password: command.Password);

            AuthToken result = await GenerateAuthToken(user: user);

            return Response.BuildServiceResponse().BuildSuccessResponse(result);
        }

        public async Task<bool> BeAnExistingRefreshToken(string refreshToken)
        {
            var filter = Builders<RefreshToken>.Filter.And(Builders<RefreshToken>.Filter.Eq(x => x.Jwt, refreshToken));

            return await _mongoDbService.Exists(filter);
        }

        public async Task<ServiceResponse> ValidateToken(ValidateTokenCommand command)
        {
            var filter = Builders<RefreshToken>.Filter.And(Builders<RefreshToken>.Filter.Eq(x => x.Jwt, command.RefreshToken));

            var refreshToken = await _mongoDbService.FindOne(filter);

            var user = await _userRepository.GetUser(userId: refreshToken.UserId);

            if (user == null)
                return Response.BuildServiceResponse().BuildErrorResponse("User not found.");

            AuthToken result = await GenerateAuthToken(user: user);

            // delete the old refresh token
            await _mongoDbService.DeleteById<RefreshToken>(refreshToken.Id);

            return Response.BuildServiceResponse().BuildSuccessResponse(result);
        }

        private async Task<AuthToken> GenerateAuthToken(User user)
        {
            var userId = user.Id;
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = _configuration["Jwt:Key"];

            var keyBytes = Encoding.ASCII.GetBytes(key);

            var lifeTime = DateTime.UtcNow.AddSeconds(120);

            string[] userClaims = await GetUserClaims(userId);

            string jwtToken = GenerateJwt(userClaims, issuer, audience, keyBytes, lifeTime);

            // create refresh token
            RefreshToken refreshToken = new()
            {
                UserId = user.Id,
            };

            refreshToken.Jwt = GenerateJwt(new[] { refreshToken.Id }, issuer, audience, keyBytes, lifeTime);

            // save the refresh token
            await _mongoDbService.InsertDocument(refreshToken);

            // return the auth token with refresh token
            var result = new AuthToken()
            {
                AccessToken = jwtToken,
                ExpiresOn = lifeTime,
                RefreshToken = refreshToken.Jwt,
            };

            return result;
        }

        private async Task<string[]> GetUserClaims(string userId)
        {
            // find user roles, then from roles, find claims, then from claims assign in token

            var roles = await _roleRepository.GetUserRoles(userId);

            var roleIds = roles.Select(r => r.RoleId).Distinct().ToArray();

            var claims = await _claimPermissionRepository.GetClaimsForRoleIds(roleIds);

            var userClaims = claims.Select(c => c.ClaimPermission).Distinct().ToArray();

            return userClaims;
        }

        private static string GenerateJwt(
            string[] userClaims,
            string? issuer,
            string? audience,
            byte[] keyBytes,
            DateTime lifeTime)
        {
            var claims = new List<Claim>();

            foreach (var claim in userClaims)
            {
                claims.Add(new Claim("Permissions", claim));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = lifeTime,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha256Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            return jwtToken;
        }

        #endregion
    }
}
