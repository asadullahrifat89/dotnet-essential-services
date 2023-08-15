using BaseModule.Application.DTOs.Responses;
using EmailModule.Application.Commands;
using EmailModule.Domain.Entities;

namespace EmailModule.Domain.Repositories.Interfaces
{
    public interface IEmailMessageRepository
    {
        Task<ServiceResponse> EnqueueEmailMessage(EnqueueEmailMessageCommand command);

        Task<List<EmailMessage>> GetEmailMessagesForSending();

        Task<bool> UpdateEmailMessageStatus(string emailMessageId, EmailSendStatus emailSendStatus);
    }
}
