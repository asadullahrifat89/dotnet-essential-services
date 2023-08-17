using Email.Domain.Entities;

namespace Email.Domain.Repositories.Interfaces
{
    public interface IEmailMessageRepository
    {
        Task<EmailMessage> EnqueueEmailMessage(EmailMessage emailMessage);

        Task<List<EmailMessage>> GetEmailMessagesForSending();

        Task<bool> UpdateEmailMessageStatus(string emailMessageId, EmailSendStatus emailSendStatus);
    }
}
