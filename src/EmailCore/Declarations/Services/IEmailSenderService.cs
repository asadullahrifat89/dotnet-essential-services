using EmailCore.Models.Entities;

namespace EmailCore.Declarations.Services
{
    public interface IEmailSenderService
    {
        Task<bool> SendEmailAsync(EmailMessage emailMessage, EmailTemplate emailTemplate);
    }
}
