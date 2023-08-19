using Base.Application.Extensions;
using Teams.CustomerEngagement.Domain.Entities;

namespace Teams.CustomerEngagement.Application.DTOs.Responses
{
    public class QuotationStatusCount
    {
        public string QuoteStatus { get; set; }

        public long Count { get; set; }

        public static QuotationStatusCount Map((QuoteStatus QuoteStatus, long Count) record)
        {
            return new QuotationStatusCount()
            {
                QuoteStatus = EnumExtensions.GetEnumDescription(record.QuoteStatus),
                Count = record.Count
            };
        }
    }
}
