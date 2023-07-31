using IdentityCore.Contracts.Declarations.Commands;
using IdentityCore.Models.Responses;

namespace IdentityCore.Contracts.Declarations.Repositories
{
    public interface IAuthTokenRepository
    {
        Task<ServiceResponse> Authenticate(AuthenticateCommand command);

        Task<bool> BeAnExistingRefreshToken(string refreshToken, string companyId);

        //Task<ServiceResponse> ValidateToken(ValidateTokenCommand command);
    }
}
