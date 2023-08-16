using BaseModule.Application.DTOs.Responses;

namespace IdentityModule.Domain.Repositories.Interfaces
{
    public interface IAuthTokenRepository
    {
        Task<ServiceResponse> Authenticate(string email, string password);

        Task<bool> BeAnExistingRefreshToken(string refreshToken);

        Task<ServiceResponse> ValidateToken(string jwt);
    }
}
