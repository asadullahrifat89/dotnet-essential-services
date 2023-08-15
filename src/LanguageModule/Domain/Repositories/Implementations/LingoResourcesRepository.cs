using BaseModule.Application.DTOs.Responses;
using BaseModule.Application.Providers.Interfaces;
using IdentityModule.Infrastructure.Services.Interfaces;
using LanguageModule.Application.Commands;
using LanguageModule.Application.Queries;
using LanguageModule.Domain.Entities;
using LanguageModule.Domain.Repositories.Interfaces;
using MongoDB.Driver;

namespace LanguageModule.Domain.Repositories.Implementations
{
    public class LingoResourcesRepository : ILingoResourcesRepository
    {
        #region Fields

        private readonly IMongoDbContextProvider _mongoDbService;
        private readonly IAuthenticationContextProviderService _authenticationContextProvider;

        #endregion

        #region Ctor

        public LingoResourcesRepository(IMongoDbContextProvider mongoDbService, IAuthenticationContextProviderService authenticationContextProvider)
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

            return _mongoDbService.Exists(filter);
        }

        public async Task<QueryRecordResponse<Dictionary<string, string>>> GetLingoResourcesInFormat(GetLingoResourcesInFormatQuery query)
        {
            var format = query.Format.ToLower();

            switch (format)
            {
                case "json":
                    return await GetLingoResourcesInJson(query);
                default:
                    return Response.BuildQueryRecordResponse<Dictionary<string, string>>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError("Format is not supported yet.", _authenticationContextProvider.GetAuthenticationContext().RequestUri));
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
