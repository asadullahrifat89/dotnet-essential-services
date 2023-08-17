using Base.Domain.Entities;
using System.Text.Json.Serialization;

namespace Email.Domain.Entities
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

        public EmailBody EmailBody { get; set; } = new EmailBody();

        public EmailBodyType EmailBodyType { get; set; }

        public string Category { get; set; } = string.Empty;

        public EmailTemplateConfiguration EmailTemplateConfiguration { get; set; } = new EmailTemplateConfiguration();

        public EmailSendStatus EmailSendStatus { get; set; } = EmailSendStatus.Pending;

        public int SendingAttempt { get; set; } = 0;
    }

    public class EmailTemplateConfiguration
    {
        public string EmailTemplateId { get; set; } = string.Empty;

        public IDictionary<string, string> TagValues { get; set; } = new Dictionary<string, string>();
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EmailSendStatus
    {
        Pending,
        Sent,
        Failed,
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EmailBodyType
    {
        NonTemplated,
        Templated,
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum EmailBodyContentType
    {
        Text,
        HTML,
    }

    public class EmailBody
    {
        public string Content { get; set; } = string.Empty;

        public EmailBodyContentType EmailBodyContentType { get; set; } = EmailBodyContentType.Text;
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
