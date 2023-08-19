using Base.Application.DTOs.Responses;
using MediatR;
using Teams.CustomerEngagement.Application.DTOs.Responses;

namespace Teams.CustomerEngagement.Application.Queries
{
    public class GetQuotationStatusCountsQuery : IRequest<QueryRecordsResponse<QuotationStatusCount>>
    {

    }
}
