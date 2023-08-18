using Base.Application.DTOs.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teams.ContentMangement.Domain.Entities;
using Teams.CustomerEngagement.Domain.Entities;

namespace Teams.CustomerEngagement.Application.Queries
{
    public class GetQuotationQuery : IRequest<QueryRecordResponse<Quotation>>
    {
        public string QuotationId { get; set; } = string.Empty;
    }
}
