using BaseModule.Application.DTOs.Responses;
using ProductSearchCriteriaModule.Domain.Entities;

namespace ProductSearchCriteriaModule.Domain.Repositories.Interfaces
{
    public interface IProductSearchCriteriaRepository
    {
        Task<ServiceResponse> AddProductSearchCriteria(ProductSearchCriteria productSearchCriteria);

        Task<ServiceResponse> UpdateProductSearchCriteria(ProductSearchCriteria productSearchCriteria);

        Task<QueryRecordResponse<ProductSearchCriteria>> GetProductSearchCriteria(string searchCriteriaId);

        Task<QueryRecordsResponse<ProductSearchCriteria>> GetProductSearchCriterias(string searchTerm, int pageIndex, int pageSize, ProductSearchCriteriaType? searchCriteriaType, SkillsetType? skillsetType);

        Task<bool> BeAnExistingProductSearchCriteria(string name);

        Task<bool> BeAnExistingProductSearchCriteriaById(string searchCriteriaId);
    }
}
