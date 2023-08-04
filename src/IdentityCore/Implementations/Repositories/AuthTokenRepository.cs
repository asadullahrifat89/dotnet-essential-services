using BaseCore.Declarations.Services;
using BaseCore.Models.Entities;
using BaseCore.Models.Responses;
using IdentityCore.Declarations.Commands;
using IdentityCore.Declarations.Repositories;
using IdentityCore.Models.Entities;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace IdentityCore.Implementations.Repositories
{
    public class AuthTokenRepository : IAuthTokenRepository
    {
        #region Fields

        private readonly IMongoDbService _mongoDbService;
        private readonly IUserRepository _userRepository;
        //private readonly IRoleRepository _roleRepository;
        //private readonly IClaimPermissionRepository _claimPermissionRepository;
        private readonly IJwtService _jwtService;
        private readonly IConfiguration _configuration;

        #endregion

        #region Ctor

        public AuthTokenRepository(
           IMongoDbService mongoDbService,
           IUserRepository userRepository,
           IConfiguration configuration,
           //IRoleRepository roleRepository,
           //IClaimPermissionRepository claimPermissionRepository,
           IJwtService jwtService)
        {
            _mongoDbService = mongoDbService;
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
            await _mongoDbService.DeleteDocuments(filter);

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

        #endregion
    }
}
