using Teams.ContentMangement.Domain.Entities;

namespace Teams.ContentMangement.Domain.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<bool> BeAnExistingProductId(string productId);

        Task<Product> AddProduct(Product product, string[] linkedSearchCriteriaIds);

        Task<Product> UpdateProduct(Product product, string[] linkedSearchCriteriaIds);

        Task<Product> GetProduct(string productId);

        Task<(long Count, Product[] Records)> GetProducts(
            string searchTerm,
            int pageIndex,
            int pageSize,
            ProductCostType? productCostType,
            EmploymentType? employmentType,
            PublishingStatus? publishingStatus,
            int? manPower,
            int? experience);
    }
}
