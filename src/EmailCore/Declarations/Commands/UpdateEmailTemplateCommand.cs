using BaseCore.Models.Responses;
using EmailCore.Models.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace EmailCore.Declarations.Commands
{
    public class UpdateEmailTemplateCommand : CreateEmailTemplateCommand
    {
        public string TemplateId { get; set; } = string.Empty;
    }
}
