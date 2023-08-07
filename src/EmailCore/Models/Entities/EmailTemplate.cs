using BaseCore.Extensions;
using BaseCore.Models.Entities;
using EmailCore.Declarations.Commands;
using System.Text.Json.Serialization;

namespace EmailCore.Models.Entities
{
    public class EmailTemplate : EntityBase
    {
        public string Name { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        public EmailBodyContentType EmailBodyContentType { get; set; } = EmailBodyContentType.Text;

        public string[] Tags { get; set; } = new string[] { };

        public static EmailTemplate Initialize(CreateEmailTemplateCommand command, AuthenticationContext authenticationContext)
        {
            var EmailTemplate = new EmailTemplate()
            {
                Name = command.Name,
                Body = command.Body,
                EmailBodyContentType = command.EmailBodyContentType,
                Tags = command.Tags,
                TimeStamp = authenticationContext.BuildCreatedByTimeStamp(),
            };

            return EmailTemplate;
        }
    }  
}
