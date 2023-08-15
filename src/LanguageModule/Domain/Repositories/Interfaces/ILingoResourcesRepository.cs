using BaseModule.Application.DTOs.Responses;
using LanguageModule.Application.Commands;
using LanguageModule.Application.Queries;
using LanguageModule.Domain.Entities;

namespace LanguageModule.Domain.Repositories.Interfaces
{
    public interface ILingoResourcesRepository
    {
        Task<ServiceResponse> AddLingoResources(List<LanguageResource> languageResources);

        Task<QueryRecordResponse<Dictionary<string, string>>> GetLingoResourcesInFormat(string appId, string format, string languageCode);

        Task<bool> BeAnExistingLanguage(string languageCode);

    }
}
