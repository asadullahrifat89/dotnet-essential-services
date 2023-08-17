using Identity.Domain.Entities;

namespace Identity.Domain.Repositories.Interfaces
{
    public interface IAuthTokenRepository
    {
        Task<AuthToken> Authenticate(string email, string password);

        Task<bool> BeAnExistingRefreshToken(string refreshToken);

        Task<AuthToken> ValidateToken(string jwt);
    }
}
