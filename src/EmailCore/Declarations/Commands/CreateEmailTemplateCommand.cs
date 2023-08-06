using BaseCore.Models.Responses;
using EmailCore.Models.Entities;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailCore.Declarations.Commands
{
    public class CreateEmailTemplateCommand : IRequest<ServiceResponse>
    {
        public string Name { get; set; } = string.Empty;

        public string Body { get; set; } = string.Empty;

        public EmailTemplateType EmailTemplateType { get; set; } = EmailTemplateType.Text;

        public string[] Tags { get; set; } = new string[] { };
    }
}
