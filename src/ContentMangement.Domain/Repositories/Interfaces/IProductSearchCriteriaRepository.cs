using ContentMangement.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContentMangement.Domain.Repositories.Interfaces
{
    public interface IProductSearchCriteriaRepository
    {
        Task<ProductSearchCriteria> AddProductSearchCriteria(ProductSearchCriteria productSearchCriteria);

        Task<ProductSearchCriteria> UpdateProductSearchCriteria(ProductSearchCriteria productSearchCriteria);

        Task<ProductSearchCriteria> GetProductSearchCriteria(string id);

        Task<(long Count, ProductSearchCriteria[] Records)> GetProductSearchCriterias(string searchTerm, int pageIndex, int pageSize, SearchCriteriaType? searchCriteriaType, SkillsetType? skillsetType);

        Task<bool> BeAnExistingProductSearchCriteria(string name);

        Task<bool> BeAnExistingProductSearchCriteriaById(string id);
    }
}
