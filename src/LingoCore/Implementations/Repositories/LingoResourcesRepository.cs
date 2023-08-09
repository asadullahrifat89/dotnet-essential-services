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

        public async Task<QueryRecordsResponse<LingoResource>> GetLingoResourcesInFormat(GetLingoResourcesInFormatQuery query)
        {
            var format = query.Format.ToLower();

            if (format.Equals("json"))
            {
                return await GetLingoResourcesInJson(query);
            }
            else if (format.Equals("xml"))
            {
                return Response.BuildQueryRecordsResponse<LingoResource>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError("XML format is not supported yet", _authenticationContextProvider.GetAuthenticationContext().RequestUri));
            }
            else
            {
                return await GetLingoResourcesInJson(query);
            }

        }

        private async Task<QueryRecordsResponse<LingoResource>> GetLingoResourcesInJson(GetLingoResourcesInFormatQuery query)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var filter = Builders<LingoResource>.Filter.Where(x => x.AppId.Equals(query.AppId) && x.LanguageCode.Equals(query.LanguageCode));

            var lingoResources = await _mongoDbService.GetDocuments(filter: filter);

            var count = await _mongoDbService.CountDocuments(filter: filter);

            return new QueryRecordsResponse<LingoResource>().BuildSuccessResponse(
               count: count,
               records: lingoResources is not null ? lingoResources.ToArray() : Array.Empty<LingoResource>(), authCtx?.RequestUri);
        }

        #endregion
    }
}
