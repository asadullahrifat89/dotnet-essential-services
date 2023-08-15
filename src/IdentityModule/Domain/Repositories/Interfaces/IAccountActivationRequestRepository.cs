using BaseModule.Application.DTOs.Responses;
using IdentityModule.Application.Commands;
using IdentityModule.Domain.Entities;

namespace IdentityModule.Domain.Repositories.Interfaces
{
    public interface IAccountActivationRequestRepository
    {
        Task<bool> BeAnExistingActivationKey(string activationKey);

        Task<ServiceResponse> CreateAccountActivationRequest(AccountActivationRequest accountActivationRequest);

        Task<ServiceResponse> VerifyAccountActivationRequest(VerifyUserAccountActivationRequestCommand command);
    }
}
