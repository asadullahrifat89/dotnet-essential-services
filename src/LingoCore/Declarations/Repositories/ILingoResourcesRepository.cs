using BaseCore.Models.Responses;
using LingoCore.Declarations.Commands;
using LingoCore.Declarations.Queries;
using LingoCore.Models.Entities;

namespace LingoCore.Declarations.Repositories
{
    public interface ILingoResourcesRepository
    {
        Task<ServiceResponse> AddLingoResources(AddLingoResourcesCommand command);

        Task<ServiceResponse> AddLingoApp(AddLingoAppCommand command);

        Task<QueryRecordResponse<LingoApp>> GetLingoApp(GetLingoAppQuery query);

        Task<QueryRecordsResponse<LingoResource>> GetLingoResourcesInFormat(GetLingoResourcesInFormatQuery query);

        Task<bool> BeAnExistingLingoApp(string appName);

        Task<bool> BeAnExistingLingoAppById(string appId);

        Task<bool> BeAnExistingLanguage(string languageCode);
      
    }
}
