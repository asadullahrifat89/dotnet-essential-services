using BaseModule.Domain.Entities;

namespace BaseModule.Services.Interfaces
{
    public interface IAuthenticationContextProviderService
    {
        AuthenticationContext GetAuthenticationContext();
    }
}
