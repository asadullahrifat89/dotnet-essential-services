using BaseCore.Models.Entities;
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
    }

    public enum EmailTemplateType
    {
        Text,
        HTML
    }
}
