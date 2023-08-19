using Base.Domain.Entities;
using System.Text.Json.Serialization;
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

        /// <summary>
        /// The assigned teams users for this quotation.
        /// </summary>
        public AssignedTeamsUser[] AssignedTeamsUsers { get; set; } = new AssignedTeamsUser[] { };

        /// <summary>
        /// List of linked blob files to this document with their types.
        /// </summary>
        public LinkedQuotationDocument[] LinkedQuotationDocuments { get; set; } = new LinkedQuotationDocument[] { };
    }

    public class LinkedQuotationDocument
    {
        /// <summary>
        /// Id of the linked document.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Name of the linked document.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Type of the linked quotation document.
        /// </summary>
        public LinkedQuotationDocumentType LinkedQuotationDocumentType { get; set; } = LinkedQuotationDocumentType.Quotation;
    }

    public class AssignedTeamsUser
    {
        /// <summary>
        /// Id of the assigned user.
        /// </summary>
        public string Id { get; set; } = string.Empty;

        /// <summary>
        /// Email of the assigned user.
        /// </summary>
        public string Email { get; set; } = string.Empty;

        /// <summary>
        /// Name of the assigned user.
        /// </summary>
        public string Name { get; set; } = string.Empty;
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
        /// The skill set type of the the product search criteria.
        /// </summary>
        public SkillsetType SkillsetType { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum LinkedQuotationDocumentType
    {
        Quotation,
        Contract,
    }


    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum QuoteStatus
    {
        Pending,
        QuotationReady,
        MeetingRequested,
        ContractReady,
        ContractSigned,
        Backlog,
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum Priority
    {
        Low,
        Medium,
        High,
    }
}
