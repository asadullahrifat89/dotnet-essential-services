using Base.Application.DTOs.Responses;
using ContentMangement.Domain.Entities;
using MediatR;

namespace ContentMangement.Application.Queries
{
    public class GetProductSearchCriteriaQuery : IRequest<QueryRecordResponse<ProductSearchCriteria>>
    {
        public string ProductSearchCriteriaId { get; set; } = string.Empty;
    }
}
