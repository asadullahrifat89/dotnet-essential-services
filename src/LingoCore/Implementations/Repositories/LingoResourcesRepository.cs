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

        public Task<ServiceResponse> AddLingoResources(AddLingoResourcesCommand command)
        {
            throw new NotImplementedException();
        }

        public Task<bool> BeAnExistingLingApp(string appName)
        {
            var filter = Builders<LingoApp>.Filter.Where(x => x.Name.ToLower().Equals(appName.ToLower()));

            return _mongoDbService.Exists<LingoApp>(filter);
        }

        public Task<QueryRecordResponse<LingoApp>> GetLingoApp(GetLingoAppQuery query)
        {
            throw new NotImplementedException();
        }

        public Task<QueryRecordResponse<LingoResource>> GetLingoResourcesInFormat(GetLingoResourcesInFormatQuery query)
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
