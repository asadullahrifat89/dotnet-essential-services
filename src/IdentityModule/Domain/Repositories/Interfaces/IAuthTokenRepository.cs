using BaseModule.Application.DTOs.Responses;
using IdentityModule.Application.Commands;

namespace IdentityModule.Domain.Repositories.Interfaces
{
    public interface IAuthTokenRepository
    {
        Task<ServiceResponse> Authenticate(AuthenticateCommand command);

        Task<bool> BeAnExistingRefreshToken(string refreshToken);

        Task<ServiceResponse> ValidateToken(ValidateTokenCommand command);
    }
}
