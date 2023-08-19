using Email.Domain.Entities;

namespace Email.Domain.Repositories.Interfaces
{
    public interface IEmailTemplateRepository
    {
        Task<EmailTemplate> CreateEmailTemplate(EmailTemplate emailTemplate);

        Task<EmailTemplate> GetEmailTemplate(string templateId);

        Task<EmailTemplate> GetEmailTemplateByPurpose(string purpose);

        Task<EmailTemplate> UpdateEmailTemplate(EmailTemplate emailTemplate);

        Task<bool> BeAnExistingEmailTemplate(string templateName);

        Task<bool> BeAnExistingEmailTemplateById(string templateId);

        Task<EmailTemplate> GetEmailTemplateById(string templateId);
    }
}
