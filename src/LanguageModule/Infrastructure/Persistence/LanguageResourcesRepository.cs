using BaseModule.Application.DTOs.Responses;
using BaseModule.Application.Providers.Interfaces;
using IdentityModule.Application.Providers.Interfaces;
using LanguageModule.Application.Commands;
using LanguageModule.Application.Queries;
using LanguageModule.Domain.Entities;
using LanguageModule.Domain.Repositories.Interfaces;
using MongoDB.Driver;

namespace LanguageModule.Infrastructure.Persistence
{
    public class LanguageResourcesRepository : ILanguageResourcesRepository
    {
        #region Fields

        private readonly IMongoDbContextProvider _mongoDbContextProvider;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public LanguageResourcesRepository(IMongoDbContextProvider mongoDbService, IAuthenticationContextProvider authenticationContextProvider)
        {
            _mongoDbContextProvider = mongoDbService;
            _authenticationContextProvider = authenticationContextProvider;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> AddLanguageResources(List<LanguageResource> languageResources)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            await _mongoDbContextProvider.InsertDocuments(languageResources);

            return Response.BuildServiceResponse().BuildSuccessResponse(languageResources, authCtx?.RequestUri);
        }

        public Task<bool> BeAnExistingLanguage(string languageCode)
        {
            var filter = Builders<LanguageResource>.Filter.Where(x => x.LanguageCode.ToLower().Equals(languageCode.ToLower()));

            return _mongoDbContextProvider.Exists(filter);
        }

        public async Task<QueryRecordResponse<Dictionary<string, string>>> GetLanguageResourcesInFormat(string appId, string format, string languageCode)
        {
            switch (format)
            {
                case "json":
                    return await GetLingoResourcesInJson(appId: appId, languageCode: languageCode);
                default:
                    return Response.BuildQueryRecordResponse<Dictionary<string, string>>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError("Format is not supported yet.", _authenticationContextProvider.GetAuthenticationContext().RequestUri));
            }
        }

        private async Task<QueryRecordResponse<Dictionary<string, string>>> GetLingoResourcesInJson(string appId, string languageCode)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var filter = Builders<LanguageResource>.Filter.Where(x => x.LanguageCode.ToLower().Equals(languageCode.ToLower()) && x.AppId == appId);

            var lingoResources = await _mongoDbContextProvider.GetDocuments(filter);

            var lingoResourcesInJson = lingoResources.ToDictionary(x => x.ResourceKey, x => x.ResourceValue);

            return Response.BuildQueryRecordResponse<Dictionary<string, string>>().BuildSuccessResponse(lingoResourcesInJson, authCtx?.RequestUri);
        }

        #endregion
    }
}
