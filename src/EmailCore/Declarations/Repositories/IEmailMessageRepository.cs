using BaseCore.Models.Responses;
using EmailCore.Declarations.Commands;
using EmailCore.Models.Entities;

namespace EmailCore.Declarations.Repositories
{
    public interface IEmailMessageRepository
    {
        Task<ServiceResponse> EnqueueEmailMessage(EnqueueEmailMessageCommand command);

        Task<List<EmailMessage>> GetEmailMessagesForSending();

        Task<bool> UpdateEmailMessageStatus(string emailMessageId, EmailSendStatus emailSendStatus);
    }
}
