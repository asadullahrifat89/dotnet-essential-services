using Base.Application.DTOs.Requests;
using Teams.CustomerEngagement.Domain.Entities;

namespace Teams.CustomerEngagement.Application.Queries
{
    public class GetQuotationsQuery : PagedRequestBase<Quotation>
    {
        public string SearchTerm { get; set; } = string.Empty;

        public Priority? Priority { get; set; } = null;

        public DateTime? FromDate { get; set; } = null;

        public DateTime? ToDate { get; set; } = null;

        public string? Location { get; set; } = null;
    }
}
