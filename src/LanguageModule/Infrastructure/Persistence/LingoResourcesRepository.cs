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
    public class LingoResourcesRepository : ILingoResourcesRepository
    {
        #region Fields

        private readonly IMongoDbContextProvider _mongoDbContextProvider;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public LingoResourcesRepository(IMongoDbContextProvider mongoDbService, IAuthenticationContextProvider authenticationContextProvider)
        {
            _mongoDbContextProvider = mongoDbService;
            _authenticationContextProvider = authenticationContextProvider;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> AddLingoResources(AddLingoResourcesCommand command)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var lingoResources = AddLingoResourcesCommand.Initialize(command, authCtx);

            await _mongoDbContextProvider.InsertDocuments(lingoResources);

            return Response.BuildServiceResponse().BuildSuccessResponse(lingoResources, authCtx?.RequestUri);
        }

        public Task<bool> BeAnExistingLanguage(string languageCode)
        {
            var filter = Builders<LanguageResource>.Filter.Where(x => x.LanguageCode.ToLower().Equals(languageCode.ToLower()));

            return _mongoDbContextProvider.Exists(filter);
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

            var filter = Builders<LanguageResource>.Filter.Where(x => x.LanguageCode.ToLower().Equals(query.LanguageCode.ToLower()));

            var lingoResources = await _mongoDbContextProvider.GetDocuments(filter);

            var lingoResourcesInJson = lingoResources.ToDictionary(x => x.ResourceKey, x => x.ResourceValue);

            return Response.BuildQueryRecordResponse<Dictionary<string, string>>().BuildSuccessResponse(lingoResourcesInJson, authCtx?.RequestUri);
        }

        #endregion
    }
}
