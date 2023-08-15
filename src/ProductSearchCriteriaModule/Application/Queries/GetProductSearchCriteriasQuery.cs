using BaseModule.Application.DTOs.Requests;
using ProductSearchCriteriaModule.Domain.Entities;

namespace ProductSearchCriteriaModule.Application.Queries
{
    public class GetProductSearchCriteriasQuery : PagedRequestBase<ProductSearchCriteria>
    {
        public string SearchTerm { get; set; } = string.Empty;

        public ProductSearchCriteriaType? SearchCriteriaType { get; set; }

        public SkillsetType? SkillsetType { get; set; }
    }
}
