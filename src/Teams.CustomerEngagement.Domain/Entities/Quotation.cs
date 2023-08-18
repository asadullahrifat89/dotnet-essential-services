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
        /// The email of the customer who submitted this quotation query.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Title of the quotation. Normally derived from the search criteria names.
        /// </summary>
        public string Title { get; set; } = string.Empty;

        /// <summary>
        /// Additional note of the quotation.
        /// </summary>
        public string Note { get; set; } = string.Empty;

        /// <summary>
        /// Location of the customer who submitted this quotation query.
        /// </summary>
        public string Location { get; set; } = string.Empty;

        /// <summary>
        /// Number of manpower requirement for this quotation.
        /// </summary>
        public int ManPower { get; set; }

        /// <summary>
        /// Years of experience requirement for this quotation.
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

        /// <summary>
        /// The submitted product search criterias for this quotation.
        /// </summary>
        public SubmittedProductSearchCriteria[] SubmittedProductSearchCriterias { get; set; } = new SubmittedProductSearchCriteria[] { };
    }

    public class SubmittedProductSearchCriteria
    {
        /// <summary>
        /// Id of the product search criteria.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Name of the product search criteria.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Based on the search criteria type, the skill set type of the the product search criteria.
        /// </summary>
        public SkillsetType SkillsetType { get; set; }
    }

    // TODO: add quotation product hash map
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
