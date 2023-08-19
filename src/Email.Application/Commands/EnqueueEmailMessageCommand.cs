using MediatR;
using Email.Domain.Entities;
using Base.Application.DTOs.Responses;
using Identity.Domain.Entities;
using Identity.Application.Extensions;

namespace Email.Application.Commands
{
    public class EnqueueEmailMessageCommand : IRequest<ServiceResponse>
    {
        public EmailContact[] To { get; set; } = new EmailContact[0];

        public EmailContact[] CC { get; set; } = new EmailContact[0];

        public EmailContact[] BCC { get; set; } = new EmailContact[0];

        public Attachment[] Attachments { get; set; } = new Attachment[0];

        public CustomVariables CustomVariables { get; set; } = new CustomVariables();

        public Headers Headers { get; set; } = new Headers();

        public string Subject { get; set; } = string.Empty;

        public EmailBody EmailBody { get; set; } = new EmailBody();

        public EmailBodyType EmailBodyType { get; set; }

        public string Category { get; set; } = string.Empty;

        public EmailTemplateConfiguration EmailTemplateConfiguration { get; set; } = new EmailTemplateConfiguration();

        public static EmailMessage Map(EnqueueEmailMessageCommand command, AuthenticationContext authenticationContext)
        {
            var emailMessage = new EmailMessage()
            {
                Subject = command.Subject,
                EmailBody = command.EmailBody,
                EmailBodyType = command.EmailBodyType,
                Category = command.Category,
                EmailTemplateConfiguration = command.EmailTemplateConfiguration,
                CC = command.CC,
                BCC = command.BCC,
                Attachments = command.Attachments,
                CustomVariables = command.CustomVariables,
                Headers = command.Headers,
                To = command.To,
                TimeStamp = authenticationContext.BuildCreatedByTimeStamp(),
            };

            return emailMessage;
        }
    }
}
