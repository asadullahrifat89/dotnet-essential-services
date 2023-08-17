using Base.Application.Providers.Interfaces;
using Identity.Application.Providers.Interfaces;
using Language.Domain.Entities;
using Language.Domain.Repositories.Interfaces;
using MongoDB.Driver;

namespace Language.Infrastructure.Persistence
{
    public class LanguageAppRepository : ILanguageAppRepository
    {
        #region Fields

        private readonly IMongoDbContextProvider _mongoDbContextProvider;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public LanguageAppRepository(IMongoDbContextProvider mongoDbService, IAuthenticationContextProvider authenticationContextProvider)
        {
            _mongoDbContextProvider = mongoDbService;
            _authenticationContextProvider = authenticationContextProvider;
        }

        #endregion

        #region Methods

        public async Task<LanguageApp> AddLingoApp(LanguageApp languageApp)
        {
            await _mongoDbContextProvider.InsertDocument(languageApp);

            return languageApp;
        }

        public async Task<LanguageApp> GetLingoApp(string appId)
        {
            var filter = Builders<LanguageApp>.Filter.Where(x => x.Id.Equals(appId));

            var languageApp = await _mongoDbContextProvider.FindOne(filter);

            return languageApp;
        }

        public Task<bool> BeAnExistingLingoApp(string appName)
        {
            var filter = Builders<LanguageApp>.Filter.Where(x => x.Name.ToLower().Equals(appName.ToLower()));

            return _mongoDbContextProvider.Exists(filter);
        }

        public Task<bool> BeAnExistingLingoAppById(string appId)
        {
            var filter = Builders<LanguageApp>.Filter.Where(x => x.Id.Equals(appId));

            return _mongoDbContextProvider.Exists(filter);
        }

        #endregion
    }
}
