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

        public async Task<ServiceResponse> AddLingoResources(AddLingoResourcesCommand command)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var lingoResources = LingoResource.Initialize(command, authCtx);

            await _mongoDbService.InsertDocuments(lingoResources);

            return Response.BuildServiceResponse().BuildSuccessResponse(lingoResources, authCtx?.RequestUri);
        }

        public Task<bool> BeAnExistingLanguage(string languageCode)
        {
            var filter = Builders<LingoResource>.Filter.Where(x => x.LanguageCode.ToLower().Equals(languageCode.ToLower()));

            return _mongoDbService.Exists<LingoResource>(filter);
        }

        public async Task<QueryRecordResponse<Dictionary<string, string>>> GetLingoResourcesInFormat(GetLingoResourcesInFormatQuery query)
        {
            var format = query.Format.ToLower();

            switch (format)
            {
                case "json":
                    return await GetLingoResourcesInJson(query);
                default:
                    return Response.BuildQueryRecordResponse<Dictionary<string, string>>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError("Format is not supported yet", _authenticationContextProvider.GetAuthenticationContext().RequestUri));
            }
        }

        private async Task<QueryRecordResponse<Dictionary<string, string>>> GetLingoResourcesInJson(GetLingoResourcesInFormatQuery query)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var filter = Builders<LingoResource>.Filter.Where(x => x.LanguageCode.ToLower().Equals(query.LanguageCode.ToLower()));

            var lingoResources = await _mongoDbService.GetDocuments(filter);

            var lingoResourcesInJson = lingoResources.ToDictionary(x => x.ResourceKey, x => x.ResourceValue);

            return Response.BuildQueryRecordResponse<Dictionary<string, string>>().BuildSuccessResponse(lingoResourcesInJson, authCtx?.RequestUri);
        }

        #endregion
    }
}
