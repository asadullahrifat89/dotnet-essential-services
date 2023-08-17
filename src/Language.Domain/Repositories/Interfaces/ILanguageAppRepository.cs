using LanguageModule.Domain.Entities;

namespace LanguageModule.Domain.Repositories.Interfaces
{
    public interface ILanguageAppRepository
    {
        Task<LanguageApp> AddLingoApp(LanguageApp languageApp);

        Task<LanguageApp> GetLingoApp(string appId);

        Task<bool> BeAnExistingLingoApp(string appName);

        Task<bool> BeAnExistingLingoAppById(string appId);
    }
}

