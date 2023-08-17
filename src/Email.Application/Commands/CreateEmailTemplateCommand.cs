using MediatR;
using Email.Domain.Entities;
using Base.Application.DTOs.Responses;
using Identity.Domain.Entities;
using Identity.Application.Extensions;

namespace Email.Application.Commands
{
    public class CreateEmailTemplateCommand : IRequest<ServiceResponse>
    {
        public string Name { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        public EmailBodyContentType EmailBodyContentType { get; set; } = EmailBodyContentType.Text;

        public string[] Tags { get; set; } = new string[] { };

        public string Purpose { get; set; } = string.Empty;

        public static EmailTemplate Initialize(CreateEmailTemplateCommand command, AuthenticationContext authenticationContext)
        {
            var EmailTemplate = new EmailTemplate()
            {
                Name = command.Name,
                Body = command.Body,
                EmailBodyContentType = command.EmailBodyContentType,
                Tags = command.Tags,
                Purpose = command.Purpose,
                TimeStamp = authenticationContext.BuildCreatedByTimeStamp(),
            };

            return EmailTemplate;
        }
    }
}
