using EmailModule.Domain.Entities;

namespace EmailModule.Application.Services.Interfaces
{
    public interface IEmailSenderService
    {
        Task<bool> SendEmail(EmailMessage emailMessage, EmailTemplate emailTemplate);

        Task<bool> SendEmailBatch(List<(EmailMessage? EmailMessage, EmailTemplate? EmailTemplate)> emailBatch, Action<(bool IsSuccess, EmailMessage? EmailMessage)> onBatchItemProcess);
    }
}
