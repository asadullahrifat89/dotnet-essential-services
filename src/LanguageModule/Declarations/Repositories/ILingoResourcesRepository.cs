using BaseModule.Application.DTOs.Responses;
using LanguageModule.Declarations.Commands;
using LanguageModule.Declarations.Queries;

namespace LanguageModule.Declarations.Repositories
{
    public interface ILingoResourcesRepository
    {
        Task<ServiceResponse> AddLingoResources(AddLingoResourcesCommand command);

        Task<QueryRecordResponse<Dictionary<string, string>>> GetLingoResourcesInFormat(GetLingoResourcesInFormatQuery query);

        Task<bool> BeAnExistingLanguage(string languageCode);

    }
}
