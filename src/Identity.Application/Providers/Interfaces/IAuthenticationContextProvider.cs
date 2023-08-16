using Identity.Domain.Entities;

namespace Identity.Application.Providers.Interfaces
{
    public interface IAuthenticationContextProvider
    {
        AuthenticationContext GetAuthenticationContext();
    }
}
