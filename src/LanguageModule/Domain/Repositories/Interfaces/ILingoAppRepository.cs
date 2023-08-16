using BaseModule.Application.DTOs.Responses;
using LanguageModule.Domain.Entities;

namespace LanguageModule.Domain.Repositories.Interfaces
{
    public interface ILingoAppRepository
    {
        Task<ServiceResponse> AddLingoApp(LanguageApp languageApp);

        Task<QueryRecordResponse<LanguageApp>> GetLingoApp(string appId);

        Task<bool> BeAnExistingLingoApp(string appName);

        Task<bool> BeAnExistingLingoAppById(string appId);
    }
}

