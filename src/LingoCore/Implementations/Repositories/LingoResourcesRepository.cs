using BaseCore.Models.Entities;
using BaseCore.Models.Responses;
using BaseCore.Services;
using LingoCore.Declarations.Commands;
using LingoCore.Declarations.Queries;
using LingoCore.Declarations.Repositories;
using LingoCore.Models.Entities;
using MongoDB.Driver;

namespace LingoCore.Implementations.Repositories
{
    public class LingoResourcesRepository : ILingoResourcesRepository
    {
        #region Fields
        
        private readonly IMongoDbService _mongoDbService;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public LingoResourcesRepository(IMongoDbService mongoDbService, IAuthenticationContextProvider authenticationContextProvider)
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

        public async Task<ServiceResponse> AddLingoResources(AddLingoResourcesCommand command)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var lingoResource = LingoResource.Initialize(command, authCtx);

            await _mongoDbService.InsertDocuments(lingoResource);

            return Response.BuildServiceResponse().BuildSuccessResponse(lingoResource, authCtx?.RequestUri);
        }

        public Task<bool> BeAnExistingLanguage(string languageCode)
        {
            var filter = Builders<LingoResource>.Filter.Where(x => x.LanguageCode.ToLower().Equals(languageCode.ToLower()));

            return _mongoDbService.Exists<LingoResource>(filter);
        }

        public Task<bool> BeAnExistingLingoApp(string appName)
        {
            var filter = Builders<LingoApp>.Filter.Where(x => x.Name.ToLower().Equals(appName.ToLower()));

            return _mongoDbService.Exists<LingoApp>(filter);
        }

        public Task<bool> BeAnExistingLingoAppById(string appId)
        {
            var filter = Builders<LingoApp>.Filter.Where(x => x.Id.Equals(appId));

            return _mongoDbService.Exists<LingoApp>(filter);
        }

        public Task<QueryRecordResponse<LingoApp>> GetLingoApp(GetLingoAppQuery query)
        {
            throw new NotImplementedException();
        }

        public async Task<QueryRecordsResponse<LingoResource>> GetLingoResourcesInFormat(GetLingoResourcesInFormatQuery query)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var filter = Builders<LingoResource>.Filter.Where(x => x.AppId.Equals(query.AppId) && x.LanguageCode.Equals(query.LanguageCode));

            var LingoResources =  await _mongoDbService.GetDocuments(filter: filter);

            var count =  await _mongoDbService.CountDocuments(filter: filter);

            return new QueryRecordsResponse<LingoResource>().BuildSuccessResponse(
               count: count,
               records: LingoResources is not null ? LingoResources.ToArray() : Array.Empty<LingoResource>(), authCtx?.RequestUri);

        }
        
        #endregion
    }
}
