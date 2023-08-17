using LanguageModule.Domain.Entities;

namespace LanguageModule.Domain.Repositories.Interfaces
{
    public interface ILanguageResourcesRepository
    {
        Task<List<LanguageResource>> AddLanguageResources(List<LanguageResource> languageResources);

        Task<Dictionary<string, string>> GetLanguageResourcesInJSONFormat(string appId, string languageCode);

        Task<bool> BeAnExistingLanguage(string languageCode);

        Task<bool> BeAnExistingLanguageCodeAndResourceKey(string languageCode, string resourceKey);
    }
}
