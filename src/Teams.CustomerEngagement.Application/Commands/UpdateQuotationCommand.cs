using Base.Application.DTOs.Responses;
using MediatR;
using Teams.CustomerEngagement.Domain.Entities;

namespace Teams.CustomerEngagement.Application.Commands
{
    public class UpdateQuotationCommand : IRequest<ServiceResponse>
    {
        public string QuotationId { get; set; } = string.Empty;

        public QuoteStatus QuoteStatus { get; set; } = QuoteStatus.Pending;

        public Priority Priority { get; set; } = Priority.Medium;

        public AssignedTeamsUser[] AssignedTeamsUsers { get; set; } = new AssignedTeamsUser[] { };

        public LinkedQuotationDocument[] LinkedQuotationDocuments { get; set; } = new LinkedQuotationDocument[] { };

        public string MeetingLink { get; set; } = string.Empty;

        public string Note { get; set; } = string.Empty;

        public static Quotation Map(UpdateQuotationCommand command)
        {
            var quotation = new Quotation()
            {
                Id = command.QuotationId,
                QuoteStatus = command.QuoteStatus,
                Priority = command.Priority,
                AssignedTeamsUsers = command.AssignedTeamsUsers,
                LinkedQuotationDocuments = command.LinkedQuotationDocuments,
                MeetingLink = command.MeetingLink,
                Note = command.Note,
            };

            return quotation;
        }
    }
}
