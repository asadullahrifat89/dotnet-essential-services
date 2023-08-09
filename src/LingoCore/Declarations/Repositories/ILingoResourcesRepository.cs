using BaseCore.Models.Responses;
using LingoCore.Declarations.Commands;
using LingoCore.Declarations.Queries;
using LingoCore.Models.Entities;

namespace LingoCore.Declarations.Repositories
{
    public interface ILingoResourcesRepository
    {
        Task<ServiceResponse> AddLingoResources(AddLingoResourcesCommand command);

        Task<QueryRecordsResponse<LingoResource>> GetLingoResourcesInFormat(GetLingoResourcesInFormatQuery query);
      
        Task<bool> BeAnExistingLanguage(string languageCode);
      
    }
}
