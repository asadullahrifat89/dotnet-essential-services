using BaseModule.Application.DTOs.Responses;
using IdentityModule.Domain.Entities;

namespace IdentityModule.Domain.Repositories.Interfaces
{
    public interface IAccountActivationRequestRepository
    {
        Task<bool> BeAnExistingActivationKey(string activationKey);

        Task<ServiceResponse> CreateAccountActivationRequest(AccountActivationRequest accountActivationRequest);

        Task<ServiceResponse> VerifyAccountActivationRequest(string email, string activationKey, string password);
    }
}
