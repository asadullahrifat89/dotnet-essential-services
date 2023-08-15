using BaseModule.Application.DTOs.Responses;
using LanguageModule.Application.Commands;
using LanguageModule.Application.Queries;

namespace LanguageModule.Domain.Repositories.Interfaces
{
    public interface ILingoResourcesRepository
    {
        Task<ServiceResponse> AddLingoResources(AddLingoResourcesCommand command);

        Task<QueryRecordResponse<Dictionary<string, string>>> GetLingoResourcesInFormat(GetLingoResourcesInFormatQuery query);

        Task<bool> BeAnExistingLanguage(string languageCode);

    }
}
