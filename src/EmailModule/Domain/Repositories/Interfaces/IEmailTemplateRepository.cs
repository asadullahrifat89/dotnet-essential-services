using BaseModule.Application.DTOs.Responses;
using EmailModule.Application.Commands;
using EmailModule.Application.Queries;
using EmailModule.Domain.Entities;

namespace EmailModule.Domain.Repositories.Interfaces
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
