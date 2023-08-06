using BaseCore.Extensions;
using BaseCore.Models.Entities;
using EmailCore.Declarations.Commands;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailCore.Models.Entities
{
    public class EmailMessage : EntityBase
    {
        public EmailContact[] To { get; set; } = new EmailContact[0];

        public EmailContact[] CC { get; set; } = new EmailContact[0];

        public EmailContact[] BCC { get; set; } = new EmailContact[0];

        public EmailContact From { get; set; } = new EmailContact();

        public Attachment[] Attachments { get; set; } = new Attachment[0];

        public CustomVariables CustomVariables { get; set; } = new CustomVariables();

        public Headers Headers { get; set; } = new Headers();

        public string Subject { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        public string Category { get; set; } = string.Empty;

        public string EmailTemplateId { get; set; } = string.Empty;

        public IDictionary<string, string> TagValues { get; set; } = new Dictionary<string, string>();

        public EmailSendStatus EmailSendStatus { get; set; } = EmailSendStatus.Pending;

        public int SendingAttempt { get; set; } = 0;

        public static EmailMessage Initialize(EnqueueEmailMessageCommand command, AuthenticationContext authenticationContext)
        {
            var emailMessage = new EmailMessage()
            {
                Subject = command.Subject,
                Body = command.Body,
                Category = command.Category,
                EmailTemplateId = command.EmailTemplateId,
                CC = command.CC,
                BCC = command.BCC,
                Attachments = command.Attachments,
                CustomVariables = command.CustomVariables,
                Headers = command.Headers,
                To = command.To,
                TagValues = command.TagValues,
                TimeStamp = authenticationContext.BuildCreatedByTimeStamp(),
            };

            return emailMessage;
        }
    }

    public enum EmailSendStatus
    {
        Pending,
        Sent,
        Failed,
    }

    public class EmailContact
    {
        public string Email { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;
    }

    public class CustomVariables
    {
        public string UserId { get; set; } = string.Empty;

        public string BatchId { get; set; } = string.Empty;
    }

    public class Headers
    {
        public string XMessageSource { get; set; } = string.Empty;
    }


    public class Attachment
    {
        public string Content { get; set; } = string.Empty;

        public string FileName { get; set; } = string.Empty;

        public string Type { get; set; } = string.Empty;

        public string Disposition { get; set; } = string.Empty;
    }
}
