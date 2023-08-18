using Base.Application.DTOs.Responses;
using MediatR;
using Teams.CustomerEngagement.Domain.Entities;

namespace Teams.CustomerEngagement.Application.Queries
{
    public class GetQuotationQuery : IRequest<QueryRecordResponse<Quotation>>
    {
        public string QuotationId { get; set; } = string.Empty;
    }
}
