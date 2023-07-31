using IdentityCore.Contracts.Declarations.Commands;
using IdentityCore.Contracts.Declarations.Repositories;
using IdentityCore.Contracts.Declarations.Services;
using IdentityCore.Models.Entities;
using IdentityCore.Models.Responses;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace IdentityCore.Contracts.Implementations.Repositories
{
    public class AuthTokenRepository : IAuthTokenRepository
    {
        #region Fields

        private readonly IMongoDbService _mongoDdService;
        private readonly IUserRepository _userRepository;
        private readonly IConfiguration _configuration;

        #endregion

        #region Ctor

        public AuthTokenRepository(
           IMongoDbService mongoDbService,
           IUserRepository userRepository,
           IConfiguration configuration)
        {
            _mongoDdService = mongoDbService;
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

            throw new NotImplementedException();
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
            await _mongoDdService.InsertDocument(refreshToken);

            // return the auth token with refresh token
            var result = new AuthToken()
            {
                AccessToken = jwtToken,
                ExpiresOn = lifeTime,
                RefreshToken = refreshToken.Jwt,
            };

            return result;
        }

        private static string GenerateJwt(string id, string? issuer, string? audience, byte[] keyBytes, DateTime lifeTime)
        {
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

        public Task<bool> BeAnExistingRefreshToken(string refreshToken, string companyId)
        {
            throw new NotImplementedException();
        }


        #endregion
    }
}
