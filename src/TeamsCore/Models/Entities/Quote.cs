using BaseCore.Models.Entities;
using TeamsCore.Models.Responses;

namespace TeamsCore.Models.Entities
{
    public class Quote : EntityBase
    {
        public string Title { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Location { get; set; } = string.Empty;

        public QuoteStatus QuoteStatus { get; set; } = QuoteStatus.Submitted;

        public int NumberOfResources { get; set; }

        public AttachedSearchCriteria[] SearchCriterias { get; set; } = new AttachedSearchCriteria[0];

        public AttachedProduct[] RecommendedProducts { get; set; } = new AttachedProduct[0];

        public string OfferUrl { get; set; } = string.Empty;

        public MeetingDetails MeetingDetails { get; set; } = new MeetingDetails();

    }

    public class MeetingDetails
    {
        public string MeetingLink { get; set; } = string.Empty;
    }

    public class AttachedProduct
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string IconUrl { get; set; } = string.Empty;
    }

    public enum QuoteStatus
    {
        Submitted,
        OfferAttached,
        MeetingSet,
        ContractReady,
        ContractSigned,
        NotInterested,
    }
}
