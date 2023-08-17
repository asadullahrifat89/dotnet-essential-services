using Base.Application.Providers.Interfaces;
using Identity.Application.Providers.Interfaces;
using Language.Domain.Entities;
using Language.Domain.Repositories.Interfaces;
using MongoDB.Driver;

namespace Language.Infrastructure.Persistence
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

        public async Task<List<LanguageResource>> AddLanguageResources(List<LanguageResource> languageResources)
        {
            await _mongoDbContextProvider.InsertDocuments(languageResources);

            return languageResources;
        }

        public Task<bool> BeAnExistingLanguageCodeAndResourceKey(string languageCode, string resourceKey)
        {
            var filter = Builders<LanguageResource>.Filter.Where(x => x.LanguageCode.ToLower().Equals(languageCode.ToLower()) && x.ResourceKey.ToLower().Equals(resourceKey.ToLower()));

            return _mongoDbContextProvider.Exists(filter);
        }

        public Task<bool> BeAnExistingLanguage(string languageCode)
        {
            var filter = Builders<LanguageResource>.Filter.Where(x => x.LanguageCode.ToLower().Equals(languageCode.ToLower()));

            return _mongoDbContextProvider.Exists(filter);
        }

        public async Task<Dictionary<string, string>> GetLanguageResourcesInJSONFormat(string appId, string languageCode)
        {
            var filter = Builders<LanguageResource>.Filter.Where(x => x.LanguageCode.ToLower().Equals(languageCode.ToLower()) && x.AppId == appId);

            var lingoResources = await _mongoDbContextProvider.GetDocuments(filter);

            var lingoResourcesInJson = lingoResources.ToDictionary(x => x.ResourceKey, x => x.ResourceValue);

            return lingoResourcesInJson;
        }

        #endregion
    }
}
