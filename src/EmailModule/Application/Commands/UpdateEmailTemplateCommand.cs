using EmailModule.Domain.Entities;
using IdentityModule.Domain.Entities;
using IdentityModule.Infrastructure.Extensions;

namespace EmailModule.Application.Commands
{
    public class UpdateEmailTemplateCommand : CreateEmailTemplateCommand
    {
        public string TemplateId { get; set; } = string.Empty;

        public static EmailTemplate Initialize(UpdateEmailTemplateCommand command, AuthenticationContext authenticationContext)
        {
            var EmailTemplate = new EmailTemplate()
            {
                Id = command.TemplateId,
                Name = command.Name,
                Body = command.Body,
                EmailBodyContentType = command.EmailBodyContentType,
                Tags = command.Tags,
                Purpose = command.Purpose,
            };

            return EmailTemplate;
        }
    }
}
