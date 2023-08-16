using BaseModule.Application.DTOs.Responses;
using ProductSearchCriteriaModule.Domain.Entities;

namespace ProductSearchCriteriaModule.Domain.Repositories.Interfaces
{
    public interface IProductSearchCriteriaRepository
    {
        Task<ServiceResponse> AddProductSearchCriteria(ProductSearchCriteria productSearchCriteria);

        Task<ServiceResponse> UpdateProductSearchCriteria(ProductSearchCriteria productSearchCriteria);

        Task<QueryRecordResponse<ProductSearchCriteria>> GetProductSearchCriteria(string productSearchCriteriaId);

        Task<QueryRecordsResponse<ProductSearchCriteria>> GetProductSearchCriterias(string searchTerm, int pageIndex, int pageSize, ProductSearchCriteriaType? productSearchCriteriaType, SkillsetType? skillsetType);

        Task<bool> BeAnExistingProductSearchCriteriaByName(string name);

        Task<bool> BeAnExistingProductSearchCriteriaById(string productSearchCriteriaId);
    }
}
