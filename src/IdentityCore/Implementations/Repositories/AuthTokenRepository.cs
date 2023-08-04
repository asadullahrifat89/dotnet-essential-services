using IdentityCore.Declarations.Commands;
using IdentityCore.Declarations.Repositories;
using IdentityCore.Declarations.Services;
using IdentityCore.Models.Entities;
using IdentityCore.Models.Responses;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace IdentityCore.Implementations.Repositories
{
    public class AuthTokenRepository : IAuthTokenRepository
    {
        #region Fields

        private readonly IMongoDbService _mongoDbService;
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IClaimPermissionRepository _claimPermissionRepository;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;

        #endregion

        #region Ctor

        public AuthTokenRepository(
           IMongoDbService mongoDbService,
           IUserRepository userRepository,
           IConfiguration configuration,
           IRoleRepository roleRepository,
           IClaimPermissionRepository claimPermissionRepository,
           IJwtService jwtService)
        {
            _mongoDbService = mongoDbService;
            _userRepository = userRepository;
            _configuration = configuration;
            _roleRepository = roleRepository;
            _claimPermissionRepository = claimPermissionRepository;
            _jwtService = jwtService;
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

            var lifeTime = DateTime.UtcNow.AddSeconds(Convert.ToInt32(_configuration["Jwt:Lifetime"]));

            ClaimPermission[] userClaims = await GetUserClaims(userId);

            string jwtToken = _jwtService.GenerateJwtToken(userId: userId, userClaims: userClaims.Select(x => x.Name).ToArray());

            // create refresh token
            RefreshToken refreshToken = new()
            {
                UserId = user.Id,
            };

            refreshToken.Jwt = _jwtService.GenerateJwtToken(userId: userId, userClaims: new[] { refreshToken.Id });

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

        private async Task<ClaimPermission[]> GetUserClaims(string userId)
        {
            // find user roles, then from roles, find claims, then from claims assign in token

            var roles = await _roleRepository.GetUserRoles(userId);

            var roleIds = roles.Select(r => r.RoleId).Distinct().ToArray();

            var roleClaimMaps = await _claimPermissionRepository.GetClaimsForRoleIds(roleIds);

            var claims = await _claimPermissionRepository.GetClaimsForClaimNames(roleClaimMaps.Select(x => x.ClaimPermission).Distinct().ToArray());

            return claims;
        }

        #endregion
    }
}
