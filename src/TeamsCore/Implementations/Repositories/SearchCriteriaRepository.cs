using BaseCore.Extensions;
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

        public async Task<QueryRecordsResponse<SearchCriteria>> GetSearchCriterias(GetSearchCriteriasQuery query)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();
            var filter = Builders<SearchCriteria>.Filter.Empty;

            if(!query.SearchTerm.IsNullOrBlank())
            {
                filter &= Builders<SearchCriteria>.Filter.Or(
                                       Builders<SearchCriteria>.Filter.Where(x => x.Name.ToLower().Contains(query.SearchTerm.ToLower())),
                                                          Builders<SearchCriteria>.Filter.Where(x => x.Description.ToLower().Contains(query.SearchTerm.ToLower())));
            }

            if (query.SearchCriteriaType.HasValue)
            {
                filter &= Builders<SearchCriteria>.Filter.Eq(x => x.SearchCriteriaType, query.SearchCriteriaType);
            }

            if (query.SkillsetType.HasValue)
            {
                filter &= Builders<SearchCriteria>.Filter.Eq(x => x.SkillsetType, query.SkillsetType);
            }

            var count = await _mongoDbService.CountDocuments(filter: filter);

            var searchCriterias = await _mongoDbService.GetDocuments(
                filter: filter,
                skip: query.PageIndex * query.PageSize,
                limit: query.PageSize);

            return new QueryRecordsResponse<SearchCriteria>().BuildSuccessResponse(
                count: count,
                records: searchCriterias is not null ? searchCriterias.Select(x => SearchCriteria.Initialize(x)).ToArray() : Array.Empty<SearchCriteria>(), authCtx?.RequestUri);
        }

        public async Task<ServiceResponse> UpdateSearchCriteria(UpdateSearchCriteriaCommand command)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var filter = Builders<SearchCriteria>.Filter.Where(x => x.Id.Equals(command.Id));

            var updateSearchCriteria = Builders<SearchCriteria>.Update
                .Set(x => x.Name, command.Name)
                .Set(x => x.Description, command.Description)
                .Set(x => x.SkillsetType, command.SkillsetType)
                .Set(x => x.SearchCriteriaType, command.SearchCriteriaType)
                .Set(x => x.TimeStamp.ModifiedOn, DateTime.UtcNow)
                .Set(x => x.TimeStamp.ModifiedBy, authCtx.User?.Id);

            await _mongoDbService.UpdateById(update: updateSearchCriteria, id: command.Id);

            return Response.BuildServiceResponse().BuildSuccessResponse(updateSearchCriteria, authCtx?.RequestUri);
        }

        #endregion
    }
}
