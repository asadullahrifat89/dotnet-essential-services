using BaseModule.Application.DTOs.Responses;
using EmailModule.Domain.Entities;

namespace EmailModule.Domain.Repositories.Interfaces
{
    public interface IEmailMessageRepository
    {
        Task<ServiceResponse> EnqueueEmailMessage(EmailMessage emailMessage);

        Task<List<EmailMessage>> GetEmailMessagesForSending();

        Task<bool> UpdateEmailMessageStatus(string emailMessageId, EmailSendStatus emailSendStatus);
    }
}
