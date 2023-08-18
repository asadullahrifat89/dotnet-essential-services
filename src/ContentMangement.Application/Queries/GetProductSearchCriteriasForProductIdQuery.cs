using Base.Application.DTOs.Requests;
using Base.Application.DTOs.Responses;
using MediatR;
using Teams.ContentMangement.Domain.Entities;

namespace Teams.ContentMangement.Application.Queries
{
    public class GetProductSearchCriteriasForProductIdQuery : IRequest<QueryRecordsResponse<ProductSearchCriteria>>
    {
        public string ProductId { get; set; } = string.Empty;
    }
}
