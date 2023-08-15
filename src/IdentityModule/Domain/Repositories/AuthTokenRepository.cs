using BaseModule.Application.DTOs.Responses;
using BaseModule.Application.Providers.Interfaces;
using IdentityModule.Application.Commands;
using IdentityModule.Application.Services.Interfaces;
using IdentityModule.Domain.Entities;
using IdentityModule.Domain.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace IdentityModule.Domain.Repositories
{
    public class AuthTokenRepository : IAuthTokenRepository
    {
        #region Fields

        private readonly IMongoDbContextProvider _mongoDbContextProvider;
        private readonly IUserRepository _userRepository;
        //private readonly IRoleRepository _roleRepository;
        //private readonly IClaimPermissionRepository _claimPermissionRepository;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;

        #endregion

        #region Ctor

        public AuthTokenRepository(
           IMongoDbContextProvider mongoDbService,
           IUserRepository userRepository,
           IConfiguration configuration,
           //IRoleRepository roleRepository,
           //IClaimPermissionRepository claimPermissionRepository,
           IJwtService jwtService)
        {
            _mongoDbContextProvider = mongoDbService;
            _userRepository = userRepository;
            _configuration = configuration;
            //_roleRepository = roleRepository;
            //_claimPermissionRepository = claimPermissionRepository;
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

            return await _mongoDbContextProvider.Exists(filter);
        }

        public async Task<ServiceResponse> ValidateToken(ValidateTokenCommand command)
        {
            var filter = Builders<RefreshToken>.Filter.And(Builders<RefreshToken>.Filter.Eq(x => x.Jwt, command.RefreshToken));

            var refreshToken = await _mongoDbContextProvider.FindOne(filter);

            var user = await _userRepository.GetUser(userId: refreshToken.UserId);

            if (user == null)
                return Response.BuildServiceResponse().BuildErrorResponse("User not found.");

            AuthToken result = await GenerateAuthToken(user: user);

            // delete the old refresh token
            await _mongoDbContextProvider.DeleteById<RefreshToken>(refreshToken.Id);

            return Response.BuildServiceResponse().BuildSuccessResponse(result);
        }

        private async Task<AuthToken> GenerateAuthToken(User user)
        {
            var userId = user.Id;

            var lifeTime = DateTime.UtcNow.AddSeconds(Convert.ToInt32(_configuration["Jwt:Lifetime"]));

            //ClaimPermission[] userClaims = await _claimPermissionRepository.GetUserClaims(userId);

            //string jwtToken = _jwtService.GenerateJwtToken(userId: userId, userClaims: userClaims.Select(x => x.Name).ToArray());

            string jwtToken = _jwtService.GenerateJwtToken(userId: userId, claims: Array.Empty<string>());

            // create refresh token
            RefreshToken refreshToken = new()
            {
                UserId = user.Id,
            };

            refreshToken.Jwt = _jwtService.GenerateJwtToken(userId: userId, claims: new[] { refreshToken.Id });

            var filter = Builders<RefreshToken>.Filter.And(Builders<RefreshToken>.Filter.Eq(x => x.UserId, userId));

            // delete old refresh tokens
            await _mongoDbContextProvider.DeleteDocuments(filter);

            // save the refresh token
            await _mongoDbContextProvider.InsertDocument(refreshToken);

            // return the auth token with refresh token
            var result = new AuthToken()
            {
                AccessToken = jwtToken,
                ExpiresOn = lifeTime,
                RefreshToken = refreshToken.Jwt,
            };

            return result;
        }

        #endregion
    }
}
