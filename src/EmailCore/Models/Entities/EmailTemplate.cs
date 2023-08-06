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
    public class EmailTemplate : EntityBase
    {
        public string Name { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        public EmailTemplateType EmailTemplateType { get; set; } = EmailTemplateType.Text;

        public string[] Tags { get; set; } = new string[] { };


        public static EmailTemplate Initialize(CreateTemplateCommand command, AuthenticationContext authenticationContext)
        {
            var EmailTemplate = new EmailTemplate()
            {
                Name = command.Name,
                Body = command.Body,
                EmailTemplateType = command.EmailTemplateType,
                Tags = command.Tags,
                TimeStamp = authenticationContext.BuildCreatedByTimeStamp(),
          
            };

            return EmailTemplate;
        }
    }

    public enum EmailTemplateType
    {
        Text,
        HTML
    }


}
