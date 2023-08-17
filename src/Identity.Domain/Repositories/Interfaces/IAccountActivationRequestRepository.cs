using Identity.Domain.Entities;

namespace Identity.Domain.Repositories.Interfaces
{
    public interface IAccountActivationRequestRepository
    {
        Task<bool> BeAnExistingActivationKey(string activationKey);

        Task<AccountActivationRequest> CreateAccountActivationRequest(AccountActivationRequest accountActivationRequest);

        Task<User> VerifyAccountActivationRequest(string email, string activationKey, string password);
    }
}
