using BaseModule.Domain.Entities;
using BaseModule.Infrastructure.Services.Interfaces;
using Microsoft.AspNetCore.Http;

namespace BaseModule.Infrastructure.Services.Implementations
{
    public class AuthenticationContextProviderService : IAuthenticationContextProviderService
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationContextProviderService(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public AuthenticationContext GetAuthenticationContext()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext is not null)
                return new AuthenticationContext((string?)httpContext.Items["RequestUri"], (UserBase?)httpContext.Items["User"], (string?)httpContext.Items["AccessToken"]);
            else
                return new AuthenticationContext();
        }
    }
}
