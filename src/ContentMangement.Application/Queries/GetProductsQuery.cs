using Base.Application.DTOs.Requests;
using Teams.ContentMangement.Domain.Entities;

namespace Teams.ContentMangement.Application.Queries
{
    public class GetProductsQuery : PagedRequestBase<Product>
    {
        public string SearchTerm { get; set; } = string.Empty;

        public ProductCostType? ProductCostType { get; set; } = null;

        public EmploymentType? EmploymentType { get; set; } = null;

        public PublishingStatus? PublishingStatus { get; set; } = null;

        public int? ManPower { get; set; } = null;

        public int? Experience { get; set; } = null;
    }
}
