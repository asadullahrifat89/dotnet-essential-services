using Base.Application.DTOs.Requests;
using ContentMangement.Domain.Entities;

namespace ContentMangement.Application.Queries
{
    public class GetProductSearchCriteriasQuery : PagedRequestBase<ProductSearchCriteria>
    {
        public string SearchTerm { get; set; } = string.Empty;

        public SearchCriteriaType? SearchCriteriaType { get; set; }

        public SkillsetType? SkillsetType { get; set; }
    }
}
