using BaseCore.Models.Requests;
using BaseCore.Models.Responses;
using TeamsCore.Models.Entities;
using TeamsCore.Models.Responses;

namespace TeamsCore.Declarations.Queries
{
    public class GetProductsQuery : PagedRequestBase<ProductResponse>
    {
        public string SearchTerm { get; set; } = string.Empty;

        public ProductCostType? ProductCostType { get; set; }
    }
}
