using BaseModule.Domain.Entities;

namespace BaseModule.Infrastructure.Services.Interfaces
{
    public interface IAuthenticationContextProviderService
    {
        AuthenticationContext GetAuthenticationContext();
    }
}
