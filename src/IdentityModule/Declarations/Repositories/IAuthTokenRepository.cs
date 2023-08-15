using BaseModule.Domain.DTOs.Responses;
using IdentityModule.Declarations.Commands;

namespace IdentityModule.Declarations.Repositories
{
    public interface IAuthTokenRepository
    {
        Task<ServiceResponse> Authenticate(AuthenticateCommand command);

        Task<bool> BeAnExistingRefreshToken(string refreshToken);

        Task<ServiceResponse> ValidateToken(ValidateTokenCommand command);
    }
}
