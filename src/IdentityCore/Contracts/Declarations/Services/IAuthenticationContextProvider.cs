using IdentityCore.Models.Entities;

namespace IdentityCore.Contracts.Declarations.Services
{
    public interface IAuthenticationContextProvider
    {
        AuthenticationContext GetAuthenticationContext();
    }
}
