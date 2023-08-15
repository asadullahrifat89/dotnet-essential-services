using BaseModule.Application.DTOs.Responses;
using BaseModule.Domain.Repositories.Interfaces;
using BaseModule.Infrastructure.Services.Interfaces;
using LanguageModule.Declarations.Commands;
using LanguageModule.Declarations.Queries;
using LanguageModule.Declarations.Repositories;
using LanguageModule.Models.Entities;
using MongoDB.Driver;

namespace LanguageModule.Implementations.Repositories
{
    public class LingoAppRepository : ILingoAppRepository
    {
        #region Fields

        private readonly IMongoDbRepository _mongoDbService;
        private readonly IAuthenticationContextProviderService _authenticationContextProvider;

        #endregion

        #region Ctor

        public LingoAppRepository(IMongoDbRepository mongoDbService, IAuthenticationContextProviderService authenticationContextProvider)
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
