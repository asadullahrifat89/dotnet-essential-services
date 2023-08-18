using Base.Application.DTOs.Requests;
using Teams.ContentMangement.Domain.Entities;

namespace Teams.ContentMangement.Application.Queries
{
    public class GetProductSearchCriteriasQuery : PagedRequestBase<ProductSearchCriteria>
    {
        public string SearchTerm { get; set; } = string.Empty;

        //public SearchCriteriaType? SearchCriteriaType { get; set; }

        public SkillsetType? SkillsetType { get; set; }
    }
}
