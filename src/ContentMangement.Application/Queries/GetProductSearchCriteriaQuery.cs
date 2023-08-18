using Base.Application.DTOs.Responses;
using MediatR;
using Teams.ContentMangement.Domain.Entities;

namespace Teams.ContentMangement.Application.Queries
{
    public class GetProductSearchCriteriaQuery : IRequest<QueryRecordResponse<ProductSearchCriteria>>
    {
        public string ProductSearchCriteriaId { get; set; } = string.Empty;
    }
}
