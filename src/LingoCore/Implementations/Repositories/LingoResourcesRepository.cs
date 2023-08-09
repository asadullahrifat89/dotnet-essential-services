using BaseCore.Models.Entities;
using BaseCore.Models.Responses;
using BaseCore.Services;
using LingoCore.Declarations.Commands;
using LingoCore.Declarations.Queries;
using LingoCore.Declarations.Repositories;
using LingoCore.Models.Entities;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;

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

        public async Task<QueryRecordResponse<Dictionary<string, string>>> GetLingoResourcesInFormat(GetLingoResourcesInFormatQuery query)
        {
            var format = query.Format.ToLower();

            if (format.Equals("json"))
            {
                return await GetLingoResourcesInJson(query);
            }
      
            else
            {
                return await GetLingoResourcesInJson(query);
            }

        }

        private async Task<QueryRecordResponse<Dictionary<string, string>>> GetLingoResourcesInJson(GetLingoResourcesInFormatQuery query)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var languageCode = query.LanguageCode.ToLower();

            var filter = Builders<LingoResource>.Filter.Where(lr => lr.LanguageCode == languageCode);

            var resources = await _mongoDbService.GetDocuments(filter);

            var result = resources.ToDictionary(x => x.ResourceKey, x => x.ResourceValue);

            var count = await _mongoDbService.CountDocuments(filter: filter);
 
            return Response.BuildQueryRecordResponse<Dictionary<string, string>>().BuildSuccessResponse(result, authCtx?.RequestUri);
        }



        #endregion
    }
}
