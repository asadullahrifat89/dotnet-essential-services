using BaseModule.Models.Responses;
using EmailModule.Models.Entities;
using MediatR;

namespace EmailModule.Declarations.Commands
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
    }
}
