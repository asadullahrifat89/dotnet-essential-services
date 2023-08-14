using BaseCore.Models.Requests;
using BaseCore.Models.Responses;
using TeamsCore.Models.Entities;

namespace TeamsCore.Declarations.Queries
{
    public class GetProductsQuery : PagedRequestBase<QueryRecordsResponse<Product>>
    {
        public string SearchTerm { get; set; } = string.Empty;

        public ProductCostType? ProductCostType { get; set; }
    }
}
