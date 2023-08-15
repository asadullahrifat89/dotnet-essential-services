using BaseModule.Application.DTOs.Responses;
using IdentityModule.Application.Commands;

namespace IdentityModule.Domain.Repositories.Interfaces
{
    public interface IAccountActivationRequestRepository
    {
        Task<bool> BeAnExistingActivationKey(string activationKey);

        Task<ServiceResponse> CreateAccountActivationRequest(SendUserAccountActivationRequestCommand command);

        Task<ServiceResponse> VerifyAccountActivationRequest(VerifyUserAccountActivationRequestCommand command);
    }
}
