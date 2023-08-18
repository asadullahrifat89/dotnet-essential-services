using Teams.ContentMangement.Domain.Entities;

namespace Teams.ContentMangement.Domain.Repositories.Interfaces
{
    public interface IProductSearchCriteriaRepository
    {
        Task<bool> BeAnExistingProductSearchCriteria(string name);

        Task<bool> BeAnExistingProductSearchCriteriaById(string id);

        Task<ProductSearchCriteria> AddProductSearchCriteria(ProductSearchCriteria productSearchCriteria);

        Task<ProductSearchCriteria> UpdateProductSearchCriteria(ProductSearchCriteria productSearchCriteria);

        Task<ProductSearchCriteria> GetProductSearchCriteria(string id);

        Task<(long Count, ProductSearchCriteria[] Records)> GetProductSearchCriterias(string searchTerm, int pageIndex, int pageSize, SearchCriteriaType? searchCriteriaType, SkillsetType? skillsetType);

        Task<(long Count, ProductSearchCriteria[] Records)> GetProductSearchCriteriasForProductId(string productId);
    }
}
