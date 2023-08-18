using ContentMangement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentMangement.Domain.Repositories.Interfaces
{
    public interface IProductRepository
    {
        Task<bool> BeAnExistingProductId(string productId);

        Task<Product> GetProduct(string productId);

        Task<Product> AddProduct(Product product, string[] linkedSearchCriteriaIds);

        Task<Product> UpdateProduct(Product product, string[] linkedSearchCriteriaIds);

        Task<(long Count, Product[] Records)> GetProducts(string searchTerm, int pageIndex, int pageSize, ProductCostType? productCostType);
    }
}
