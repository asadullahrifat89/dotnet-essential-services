using BaseModule.Application.DTOs.Responses;
using LanguageModule.Domain.Entities;

namespace LanguageModule.Domain.Repositories.Interfaces
{
    public interface ILanguageResourcesRepository
    {
        Task<ServiceResponse> AddLanguageResources(List<LanguageResource> languageResources);

        Task<QueryRecordResponse<Dictionary<string, string>>> GetLanguageResourcesInFormat(string appId, string format, string languageCode);

        Task<bool> BeAnExistingLanguage(string languageCode);

        Task<bool> BeAnExistingLanguageCodeAndResourceKey(string languageCode, string resourceKey);
    }
}
