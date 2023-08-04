using BaseCore.Models.Responses;
using IdentityCore.Declarations.Commands;

namespace IdentityCore.Declarations.Repositories
{
    public interface IAuthTokenRepository
    {
        Task<ServiceResponse> Authenticate(AuthenticateCommand command);

        Task<bool> BeAnExistingRefreshToken(string refreshToken);

        Task<ServiceResponse> ValidateToken(ValidateTokenCommand command);
    }
}
