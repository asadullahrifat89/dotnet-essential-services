using IdentityCore.Contracts.Declarations.Commands;
using IdentityCore.Contracts.Declarations.Repositories;
using IdentityCore.Contracts.Declarations.Services;
using IdentityCore.Contracts.Implementations.Services;
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
        private readonly IConfiguration _configuration;

        #endregion

        #region Ctor

        public AuthTokenRepository(
           IMongoDbService mongoDbService,
           IUserRepository userRepository,
           IConfiguration configuration)
        {
            _mongoDbService = mongoDbService;
            _userRepository = userRepository;
            _configuration = configuration;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> Authenticate(AuthenticateCommand command)
        {
            var user = await _userRepository.GetUser(userEmail: command.Email, password: command.Password);

            AuthToken result = await GenerateAuthToken(user: user);

            return Response.Build().BuildSuccessResponse(result);
        }

        public Task<bool> BeAnExistingRefreshToken(string refreshToken, string companyId)
        {
            throw new NotImplementedException();
        }

        public async Task<ServiceResponse> ValidateToken(ValidateTokenCommand command)
        {
            var filter = Builders<RefreshToken>.Filter.And(Builders<RefreshToken>.Filter.Eq(x => x.Jwt, command.RefreshToken));

            var refreshToken = await _mongoDbService.FindOne(filter);

            var user = await _userRepository.GetUser(userId: refreshToken.UserId);

            if (user == null)
                return Response.Build().BuildErrorResponse("User not found.");

            AuthToken result = await GenerateAuthToken(user: user);

            // delete the old refresh token
            await _mongoDbService.DeleteById<RefreshToken>(refreshToken.Id);

            return Response.Build().BuildSuccessResponse(result);
        }

        private async Task<AuthToken> GenerateAuthToken(User user)
        {
            var userId = user.Id;
            var issuer = _configuration["Jwt:Issuer"];
            var audience = _configuration["Jwt:Audience"];
            var key = _configuration["Jwt:Key"];

            var keyBytes = Encoding.ASCII.GetBytes(key);

            var lifeTime = DateTime.UtcNow.AddMinutes(2);

            // TODO: find user roles, then from roles, find claims, then from claims assign in token

            string jwtToken = GenerateJwt(userId, issuer, audience, keyBytes, lifeTime);

            // create refresh token
            RefreshToken refreshToken = new()
            {
                UserId = user.Id,
            };

            refreshToken.Jwt = GenerateJwt(refreshToken.Id, issuer, audience, keyBytes, lifeTime);

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

        private static string GenerateJwt(
            string id,
            string? issuer,
            string? audience,
            byte[] keyBytes,
            DateTime lifeTime)
        {
            // TODO: ultimately the claims will be dynamic and be sent via a function param

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim("Id", id),
                }),
                Expires = lifeTime,
                Issuer = issuer,
                Audience = audience,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(keyBytes), SecurityAlgorithms.HmacSha512Signature)
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var jwtToken = tokenHandler.WriteToken(token);

            return jwtToken;
        }

        #endregion
    }
}
