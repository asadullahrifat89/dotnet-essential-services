using BaseCore.Models.Requests;
using TeamsCore.Models.Entities;

namespace TeamsCore.Declarations.Queries
{
    public class GetSearchCriteriasQuery : PagedRequestBase<SearchCriteria>
    {   
        public string? SearchTerm { get; set; } = string.Empty;

        public SearchCriteriaType? SearchCriteriaType { get; set; }

        public SkillsetType? SkillsetType { get; set; }
    }
}
