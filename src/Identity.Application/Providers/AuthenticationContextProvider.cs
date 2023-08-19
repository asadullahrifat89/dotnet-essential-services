using Identity.Application.Providers.Interfaces;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Identity.Application.Providers
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
            {
                var requestUri = (string?)httpContext.Items["RequestUri"];
                var user = (UserBase?)httpContext.Items["User"];
                var accessToken = (string?)httpContext.Items["AccessToken"];

                return new AuthenticationContext(requestUri ?? "", user ?? new UserBase(), accessToken ?? "");
            }
            else
                return new AuthenticationContext();
        }
    }
}
