using BaseCore.Models.Responses;
using TeamsCore.Declarations.Commands;

namespace TeamsCore.Declarations.Repositories
{
    public interface ISearchCriteriaRepository
    {
        Task<ServiceResponse> AddSearchCriteria(AddSearchCriteriaCommand command);

        Task<bool> BeAnExistingSearchCriteria(string searchCriteria);
    }
}
