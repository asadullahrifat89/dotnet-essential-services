using IdentityCore.Declarations.Services;
using IdentityCore.Models.Entities;
using Microsoft.AspNetCore.Http;

namespace IdentityCore.Implementations.Services
{
    public class AuthenticationContextProvider : IAuthenticationContextProvider
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthenticationContextProvider(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public AuthenticationContext GetAuthenticationContext()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext is not null)
                return new AuthenticationContext((string?)httpContext.Items["RequestUri"], (User?)httpContext.Items["User"], (string?)httpContext.Items["AccessToken"]);
            else
                return new AuthenticationContext();
        }
    }
}
