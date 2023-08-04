using BaseCore.Models.Entities;

namespace BaseCore.Services
{
    public interface IAuthenticationContextProvider
    {
        AuthenticationContext GetAuthenticationContext();
    }
}
