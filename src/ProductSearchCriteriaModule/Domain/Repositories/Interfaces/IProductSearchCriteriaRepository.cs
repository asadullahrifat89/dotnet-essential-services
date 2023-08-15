using BaseModule.Application.DTOs.Responses;
using SearchCriteriaModule.Domain.Entities;

namespace SearchCriteriaModule.Domain.Repositories.Interfaces
{
    public interface IProductSearchCriteriaRepository
    {
        Task<ServiceResponse> AddSearchCriteria(ProductSearchCriteria searchCriteria);

        Task<ServiceResponse> UpdateSearchCriteria(ProductSearchCriteria searchCriteria);

        Task<QueryRecordResponse<ProductSearchCriteria>> GetSearchCriteria(string searchCriteriaId);

        Task<QueryRecordsResponse<ProductSearchCriteria>> GetSearchCriterias(string searchTerm, int pageIndex, int pageSize, SearchCriteriaType? searchCriteriaType, SkillsetType? skillsetType);

        Task<bool> BeAnExistingSearchCriteria(string searchCriteria);

        Task<bool> BeAnExistingSearchCriteriaById(string searchCriteriaId);
    }
}
