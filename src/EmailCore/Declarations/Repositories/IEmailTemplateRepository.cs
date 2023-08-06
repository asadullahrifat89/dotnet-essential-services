using BaseCore.Models.Responses;
using EmailCore.Declarations.Commands;
using EmailCore.Models.Entities;
using MongoDB.Driver;
using EmailCore.Declarations.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailCore.Declarations.Repositories
{
    public interface IEmailTemplateRepository
    {
        Task<ServiceResponse> CreateEmailTemplate(CreateEmailTemplateCommand command);

        Task<QueryRecordResponse<EmailTemplate>> GetEmailTemplate(GetEmailTemplateQuery query);

        Task<ServiceResponse> UpdateEmailTemplate(UpdateEmailTemplateCommand command);

        Task<bool> BeAnExistingEmailTemplate(string templateName);

        Task<bool> BeAnExistingEmailTemplateById(string templateId);
    }
}
