using BaseModule.Domain.DTOs.Responses;
using LanguageModule.Declarations.Commands;
using LanguageModule.Declarations.Queries;
using LanguageModule.Models.Entities;

namespace LanguageModule.Declarations.Repositories
{
    public interface ILingoAppRepository
    {
        Task<ServiceResponse> AddLingoApp(AddLingoAppCommand command);

        Task<QueryRecordResponse<LingoApp>> GetLingoApp(GetLingoAppQuery query);

        Task<bool> BeAnExistingLingoApp(string appName);

        Task<bool> BeAnExistingLingoAppById(string appId);
    }
}

