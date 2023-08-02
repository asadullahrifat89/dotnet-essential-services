using IdentityCore.Contracts.Declarations.Services;
using IdentityCore.Models.Entities;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Contracts.Implementations.Services
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
                return new AuthenticationContext((string?)httpContext.Items["RequestUri"], (User?)httpContext.Items["User"]);
            else
                return new AuthenticationContext();
        }
    }
}
