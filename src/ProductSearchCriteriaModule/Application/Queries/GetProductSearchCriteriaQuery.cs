using BaseModule.Application.DTOs.Responses;
using MediatR;
using ProductSearchCriteriaModule.Domain.Entities;

namespace ProductSearchCriteriaModule.Application.Queries
{
    public class GetProductSearchCriteriaQuery : IRequest<QueryRecordResponse<ProductSearchCriteria>>
    {
        public string ProductSearchCriteriaId { get; set; } = string.Empty;
    }
}
