using IdentityCore.Models.Entities;

namespace IdentityCore.Declarations.Services
{
    public interface IAuthenticationContextProvider
    {
        AuthenticationContext GetAuthenticationContext();
    }
}
