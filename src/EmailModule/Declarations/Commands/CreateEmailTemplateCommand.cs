using BaseModule.Application.DTOs.Responses;
using EmailModule.Models.Entities;
using MediatR;

namespace EmailModule.Declarations.Commands
{
    public class CreateEmailTemplateCommand : IRequest<ServiceResponse>
    {
        public string Name { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        public EmailBodyContentType EmailBodyContentType { get; set; } = EmailBodyContentType.Text;

        public string[] Tags { get; set; } = new string[] { };

        public string Purpose { get; set; } = string.Empty;
    }
}
