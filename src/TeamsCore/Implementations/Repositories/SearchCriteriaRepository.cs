using BaseCore.Models.Responses;
using BaseCore.Services;
using MongoDB.Driver;
using TeamsCore.Declarations.Commands;
using TeamsCore.Declarations.Queries;
using TeamsCore.Declarations.Repositories;
using TeamsCore.Models.Entities;

namespace TeamsCore.Implementations.Repositories
{
    public class SearchCriteriaRepository : ISearchCriteriaRepository
    {
        #region Fields

        private readonly IMongoDbService _mongoDbService;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public SearchCriteriaRepository(IMongoDbService mongoDbService, IAuthenticationContextProvider authenticationContextProvider)
        {
            _mongoDbService = mongoDbService;
            _authenticationContextProvider = authenticationContextProvider;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> AddSearchCriteria(AddSearchCriteriaCommand command)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var searchCriteria = SearchCriteria.Initialize(command, authCtx);

            await _mongoDbService.InsertDocument(searchCriteria);

            return Response.BuildServiceResponse().BuildSuccessResponse(searchCriteria, authCtx?.RequestUri);
        }

        public async Task<bool> BeAnExistingSearchCriteria(string searchCriteria)
        {
            var filter = Builders<SearchCriteria>.Filter.Where(x => x.Name.ToLower().Equals(searchCriteria.ToLower()));

            return await _mongoDbService.Exists<SearchCriteria>(filter);
        }

        public async Task<bool> BeAnExistingSearchCriteriaById(string searchCriteriaId)
        {
            var filter = Builders<SearchCriteria>.Filter.Where(x => x.Id.Equals(searchCriteriaId));

            return await _mongoDbService.Exists<SearchCriteria>(filter);
        }

        public async Task<QueryRecordResponse<SearchCriteria>> GetSearchCriteria(GetSearchCriteriaQuery request)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var filter = Builders<SearchCriteria>.Filter.Where(x => x.Id.Equals(request.SearchCriteriaId));

            var result = await _mongoDbService.FindOne(filter);

            return Response.BuildQueryRecordResponse<SearchCriteria>().BuildSuccessResponse(result, authCtx?.RequestUri);
        }

        #endregion
    }
}
