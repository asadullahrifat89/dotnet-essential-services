using BaseCore.Models.Entities;

namespace BaseCore.Declarations.Services
{
    public interface IAuthenticationContextProvider
    {
        AuthenticationContext GetAuthenticationContext();
    }
}
