using BaseCore.Models.Responses;
using TeamsCore.Declarations.Commands;
using TeamsCore.Declarations.Queries;
using TeamsCore.Models.Entities;

namespace TeamsCore.Declarations.Repositories
{
    public interface ISearchCriteriaRepository
    {
        Task<ServiceResponse> AddSearchCriteria(AddSearchCriteriaCommand command);

        Task<ServiceResponse> UpdateSearchCriteria(UpdateSearchCriteriaCommand command);

        Task<QueryRecordResponse<SearchCriteria>> GetSearchCriteria(GetSearchCriteriaQuery request);

        Task<QueryRecordsResponse<SearchCriteria>> GetSearchCriterias(GetSearchCriteriasQuery request);

        Task<bool> BeAnExistingSearchCriteria(string searchCriteria);

        Task<bool> BeAnExistingSearchCriteriaById(string searchCriteriaId);
    }
}
