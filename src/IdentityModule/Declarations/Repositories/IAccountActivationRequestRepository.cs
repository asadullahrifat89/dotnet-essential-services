using BaseModule.Application.DTOs.Responses;
using IdentityModule.Declarations.Commands;

namespace IdentityModule.Declarations.Repositories
{
    public interface IAccountActivationRequestRepository
    {
        Task<bool> BeAnExistingActivationKey(string activationKey);

        Task<ServiceResponse> CreateAccountActivationRequest(SendUserAccountActivationRequestCommand command);

        Task<ServiceResponse> VerifyAccountActivationRequest(VerifyUserAccountActivationRequestCommand command);
    }
}
