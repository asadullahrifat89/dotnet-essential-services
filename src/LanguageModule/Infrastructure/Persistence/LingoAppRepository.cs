using BaseModule.Application.DTOs.Responses;
using BaseModule.Application.Providers.Interfaces;
using IdentityModule.Application.Providers.Interfaces;
using LanguageModule.Application.Commands;
using LanguageModule.Application.Queries;
using LanguageModule.Domain.Entities;
using LanguageModule.Domain.Repositories.Interfaces;
using MongoDB.Driver;

namespace LanguageModule.Infrastructure.Persistence
{
    public class LingoAppRepository : ILingoAppRepository
    {
        #region Fields

        private readonly IMongoDbContextProvider _mongoDbContextProvider;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public LingoAppRepository(IMongoDbContextProvider mongoDbService, IAuthenticationContextProvider authenticationContextProvider)
        {
            _mongoDbContextProvider = mongoDbService;
            _authenticationContextProvider = authenticationContextProvider;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> AddLingoApp(LanguageApp languageApp)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();
            
            await _mongoDbContextProvider.InsertDocument(languageApp);

            return Response.BuildServiceResponse().BuildSuccessResponse(languageApp, authCtx?.RequestUri);
        }

        public async Task<QueryRecordResponse<LanguageApp>> GetLingoApp(string appId)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var filter = Builders<LanguageApp>.Filter.Where(x => x.Id.Equals(appId));

            var user = await _mongoDbContextProvider.FindOne(filter);

            return Response.BuildQueryRecordResponse<LanguageApp>().BuildSuccessResponse(user, authCtx?.RequestUri);

        }

        public Task<bool> BeAnExistingLingoApp(string appName)
        {
            var filter = Builders<LanguageApp>.Filter.Where(x => x.Name.ToLower().Equals(appName.ToLower()));

            return _mongoDbContextProvider.Exists(filter);
        }

        public Task<bool> BeAnExistingLingoAppById(string appId)
        {
            var filter = Builders<LanguageApp>.Filter.Where(x => x.Id.Equals(appId));

            return _mongoDbContextProvider.Exists(filter);
        }

        #endregion
    }
}
