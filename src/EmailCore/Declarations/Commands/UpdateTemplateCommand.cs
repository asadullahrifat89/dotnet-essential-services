using BaseCore.Models.Responses;
using EmailCore.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EmailCore.Declarations.Commands
{
    public class UpdateTemplateCommand : IRequest<ServiceResponse>
    {
        public string TemplateId { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        public EmailTemplateType EmailTemplateType { get; set; } = EmailTemplateType.Text;

        public string[] Tags { get; set; } = new string[] { };
    }
}
