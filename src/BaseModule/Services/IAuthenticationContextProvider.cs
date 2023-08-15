using BaseModule.Models.Entities;

namespace BaseModule.Services
{
    public interface IAuthenticationContextProvider
    {
        AuthenticationContext GetAuthenticationContext();
    }
}
