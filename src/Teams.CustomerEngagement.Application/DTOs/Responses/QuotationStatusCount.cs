using Teams.CustomerEngagement.Domain.Entities;

namespace Teams.CustomerEngagement.Application.DTOs.Responses
{
    public class QuotationStatusCount
    {
        public QuoteStatus QuoteStatus { get; set; }

        public long Count { get; set; }

        public static QuotationStatusCount Initialize((QuoteStatus QuoteStatus, long Count) record)
        {
            return new QuotationStatusCount()
            {
                QuoteStatus = record.QuoteStatus,
                Count = record.Count
            };
        }
    }
}
