using BaseModule.Application.DTOs.Responses;
using BaseModule.Application.Providers.Interfaces;
using IdentityModule.Application.Providers.Interfaces;
using LanguageModule.Application.Commands;
using LanguageModule.Application.Queries;
using LanguageModule.Domain.Entities;
using LanguageModule.Domain.Repositories.Interfaces;
using MongoDB.Driver;

namespace LanguageModule.Domain.Repositories
{
    public class LingoAppRepository : ILingoAppRepository
    {
        #region Fields

        private readonly IMongoDbContextProvider _mongoDbService;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public LingoAppRepository(IMongoDbContextProvider mongoDbService, IAuthenticationContextProvider authenticationContextProvider)
        {
            _mongoDbService = mongoDbService;
            _authenticationContextProvider = authenticationContextProvider;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> AddLingoApp(AddLingoAppCommand command)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var lingoApp = LingoApp.Initialize(command, authCtx);

            await _mongoDbService.InsertDocument(lingoApp);

            return Response.BuildServiceResponse().BuildSuccessResponse(lingoApp, authCtx?.RequestUri);
        }

        public async Task<QueryRecordResponse<LingoApp>> GetLingoApp(GetLingoAppQuery query)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var filter = Builders<LingoApp>.Filter.Where(x => x.Id.Equals(query.AppId));

            var user = await _mongoDbService.FindOne(filter);

            return Response.BuildQueryRecordResponse<LingoApp>().BuildSuccessResponse(user, authCtx?.RequestUri);

        }

        public Task<bool> BeAnExistingLingoApp(string appName)
        {
            var filter = Builders<LingoApp>.Filter.Where(x => x.Name.ToLower().Equals(appName.ToLower()));

            return _mongoDbService.Exists(filter);
        }

        public Task<bool> BeAnExistingLingoAppById(string appId)
        {
            var filter = Builders<LingoApp>.Filter.Where(x => x.Id.Equals(appId));

            return _mongoDbService.Exists(filter);
        }

        #endregion
    }
}
