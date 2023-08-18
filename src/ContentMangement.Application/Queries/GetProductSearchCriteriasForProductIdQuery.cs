using Base.Application.DTOs.Requests;
using Teams.ContentMangement.Domain.Entities;

namespace Teams.ContentMangement.Application.Queries
{
    public class GetProductSearchCriteriasForProductIdQuery : PagedRequestBase<ProductSearchCriteria>
    {
        public string ProductId { get; set; } = string.Empty;
    }
}
