using Base.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Teams.ContentMangement.Domain.Entities;

namespace Teams.CustomerEngagement.Domain.Entities
{
    public class Quotation : EntityBase
    {
        /// <summary>
        /// Name of the quotation. Normally derived from the search criterias.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Additional description of the quotation.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// The email of the customer who submitted this quotation query.
        /// </summary>
        public string CustomerEmail { get; set; } = string.Empty;

        /// <summary>
        /// Location of the customer who submitted this quotation query.
        /// </summary>
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// Number of manpower requirements for this quotation.
        /// </summary>
        public int ManPower { get; set; }

        /// <summary>
        /// Years of experience requirements for this quotation.
        /// </summary>
        public int Experience { get; set; } = 0;

        /// <summary>
        /// Preferred employment types of the customer.
        /// </summary>
        public EmploymentType[] EmploymentTypes { get; set; } = new EmploymentType[] { };

        /// <summary>
        /// Status of the quotation.
        /// </summary>
        public QuoteStatus QuoteStatus { get; set; } = QuoteStatus.Pending;

        /// <summary>
        /// Priority of the quotation.
        /// </summary>
        public Priority Priority { get; set; } = Priority.Medium;

        /// <summary>
        /// Meeting link for the quotation.
        /// </summary>
        public string MeetingLink { get; set; } = string.Empty;
    }
    
    // TODO: add quotation search criteria hash map
    // TODO: add quotation and document hash map

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum AttachedDocumentType
    {
        Quotation,
        Contract,
        Misc,
    }


    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum QuoteStatus
    {
        Pending,
        QuotationReady,
        MeetingRequested,
        ContractReady,
        ContractSigned
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Priority
    {
        Low,
        Medium,
        High,
    }
}
