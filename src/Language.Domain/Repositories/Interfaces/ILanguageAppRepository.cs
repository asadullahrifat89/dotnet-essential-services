using Language.Domain.Entities;

namespace Language.Domain.Repositories.Interfaces
{
    public interface ILanguageAppRepository
    {
        Task<LanguageApp> AddLingoApp(LanguageApp languageApp);

        Task<LanguageApp> GetLingoApp(string appId);

        Task<bool> BeAnExistingLingoApp(string appName);

        Task<bool> BeAnExistingLingoAppById(string appId);
    }
}

