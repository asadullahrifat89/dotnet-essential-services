using BaseCore.Models.Responses;
using EmailCore.Declarations.Commands;
using EmailCore.Models.Entities;
using MongoDB.Driver;
using EmailCore.Declarations.Queries;
using EmailCore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailCore.Declarations.Repositories
{
    public interface IEmailRepository
    {
        Task<ServiceResponse> CreateTemplate(CreateTemplateCommand command);
        Task<QueryRecordResponse<EmailTemplate>> GetEmailTemplate(GetEmailTemplateQuery query);

        Task<ServiceResponse> UpdateTemplate(UpdateTemplateCommand command);

        Task<bool> BeAnExistingEmailTemplate(string templateName);
        Task<bool> BeAnExistingEmailTemplateById(string templateId);
    }
}
