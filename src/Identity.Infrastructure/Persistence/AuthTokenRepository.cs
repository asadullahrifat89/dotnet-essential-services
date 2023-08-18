using Base.Infrastructure.Providers.Interfaces;
using Identity.Application.Services.Interfaces;
using Identity.Domain.Entities;
using Identity.Domain.Repositories.Interfaces;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Identity.Infrastructure.Persistence
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

        public async Task<AuthToken> Authenticate(string email, string password)
        {
            var user = await _userRepository.GetUser(userEmail: email, password: password);

            AuthToken result = await GenerateAuthToken(user: user);

            return result;
        }

        public async Task<bool> BeAnExistingRefreshToken(string refreshToken)
        {
            var filter = Builders<RefreshToken>.Filter.And(Builders<RefreshToken>.Filter.Eq(x => x.Jwt, refreshToken));

            return await _mongoDbContextProvider.Exists(filter);
        }

        public async Task<AuthToken> ValidateToken(string jwt)
        {
            var filter = Builders<RefreshToken>.Filter.And(Builders<RefreshToken>.Filter.Eq(x => x.Jwt, jwt));

            var refreshToken = await _mongoDbContextProvider.FindOne(filter);

            var user = await _userRepository.GetUserById(userId: refreshToken.UserId);

            //if (user == null)
            //    return Response.BuildServiceResponse().BuildErrorResponse("User not found.");

            AuthToken result = await GenerateAuthToken(user: user);

            // delete the old refresh token
            await _mongoDbContextProvider.DeleteById<RefreshToken>(refreshToken.Id);

            return result;
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
