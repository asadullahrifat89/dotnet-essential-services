using BaseModule.Application.DTOs.Responses;
using LanguageModule.Application.Commands;
using LanguageModule.Application.Queries;
using LanguageModule.Domain.Entities;

namespace LanguageModule.Domain.Repositories.Interfaces
{
    public interface ILingoAppRepository
    {
        Task<ServiceResponse> AddLingoApp(AddLingoAppCommand command);

        Task<QueryRecordResponse<LingoApp>> GetLingoApp(GetLingoAppQuery query);

        Task<bool> BeAnExistingLingoApp(string appName);

        Task<bool> BeAnExistingLingoAppById(string appId);
    }
}

