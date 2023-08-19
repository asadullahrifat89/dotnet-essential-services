using Base.Application.DTOs.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teams.CustomerEngagement.Application.DTOs.Responses;

namespace Teams.CustomerEngagement.Application.Queries
{
    public class GetQuotationStatusCountsQuery : IRequest<QueryRecordsResponse<QuotationStatusCount>>
    {

    }
}
