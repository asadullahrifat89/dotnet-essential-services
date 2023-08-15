using BaseModule.Domain.DTOs.Responses;
using EmailModule.Declarations.Commands;
using EmailModule.Models.Entities;

namespace EmailModule.Declarations.Repositories
{
    public interface IEmailMessageRepository
    {
        Task<ServiceResponse> EnqueueEmailMessage(EnqueueEmailMessageCommand command);

        Task<List<EmailMessage>> GetEmailMessagesForSending();

        Task<bool> UpdateEmailMessageStatus(string emailMessageId, EmailSendStatus emailSendStatus);
    }
}
