using IdentityModule.Domain.Entities;

namespace IdentityModule.Application.Providers.Interfaces
{
    public interface IAuthenticationContextProvider
    {
        AuthenticationContext GetAuthenticationContext();
    }
}
