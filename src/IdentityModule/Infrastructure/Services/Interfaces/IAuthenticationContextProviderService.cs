using IdentityModule.Domain.Entities;

namespace IdentityModule.Infrastructure.Services.Interfaces
{
    public interface IAuthenticationContextProviderService
    {
        AuthenticationContext GetAuthenticationContext();
    }
}
