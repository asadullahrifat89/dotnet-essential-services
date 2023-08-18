using Base.Application.DTOs.Requests;
using Teams.ContentMangement.Application.DTOs.Responses;
using Teams.ContentMangement.Domain.Entities;

namespace Teams.CustomerEngagement.Application.Queries
{
    public class GetProductRecommendationsQuery : PagedRequestBase<ProductRecommendationResponse>
    {
        public string[] ProductSearchCriteriaIds { get; set; } = new string[] { };

        public EmploymentType[]? EmploymentTypes { get; set; } = null;

        public int? MinimumManPower { get; set; } = null;

        public int? MinimumExperience { get; set; } = null;
    }
}
