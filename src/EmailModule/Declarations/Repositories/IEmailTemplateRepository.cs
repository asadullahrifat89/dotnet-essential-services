using BaseModule.Models.Responses;
using EmailModule.Declarations.Commands;
using EmailModule.Declarations.Queries;
using EmailModule.Models.Entities;

namespace EmailModule.Declarations.Repositories
{
    public interface IEmailTemplateRepository
    {
        Task<ServiceResponse> CreateEmailTemplate(CreateEmailTemplateCommand command);

        Task<QueryRecordResponse<EmailTemplate>> GetEmailTemplate(GetEmailTemplateQuery query);

        Task<ServiceResponse> UpdateEmailTemplate(UpdateEmailTemplateCommand command);

        Task<bool> BeAnExistingEmailTemplate(string templateName);

        Task<bool> BeAnExistingEmailTemplateById(string templateId);

        Task<EmailTemplate> GetEmailTemplate(string templateId);
    }
}
