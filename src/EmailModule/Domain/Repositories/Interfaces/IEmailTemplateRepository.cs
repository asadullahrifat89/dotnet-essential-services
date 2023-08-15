using BaseModule.Application.DTOs.Responses;
using EmailModule.Application.Commands;
using EmailModule.Application.Queries;
using EmailModule.Domain.Entities;

namespace EmailModule.Domain.Repositories.Interfaces
{
    public interface IEmailTemplateRepository
    {
        Task<ServiceResponse> CreateEmailTemplate(EmailTemplate emailTemplate);

        Task<QueryRecordResponse<EmailTemplate>> GetEmailTemplate(string templateId);

        Task<ServiceResponse> UpdateEmailTemplate(UpdateEmailTemplateCommand command);

        Task<bool> BeAnExistingEmailTemplate(string templateName);

        Task<bool> BeAnExistingEmailTemplateById(string templateId);

        Task<EmailTemplate> GetEmailTemplateById(string templateId);
    }
}
