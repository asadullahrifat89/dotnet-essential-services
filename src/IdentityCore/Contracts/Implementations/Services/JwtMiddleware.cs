using IdentityCore.Attributes;
using IdentityCore.Contracts.Declarations.Repositories;
using IdentityCore.Contracts.Declarations.Services;
using IdentityCore.Extensions;
using Microsoft.AspNetCore.Http;
using System.Net.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Http.Extensions;
using IdentityCore.Models.Entities;

namespace IdentityCore.Contracts.Implementations.Services
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;

        public JwtMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(
            HttpContext httpContext,
            IUserRepository userRepository,
            IRoleRepository roleRepository,
            IClaimPermissionRepository claimPermissionRepository,
            IJwtService jwtService)
        {
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var requestUri = httpContext.Request.Path.Value;

            httpContext.Items["RequestUri"] = requestUri;

            if (!token.IsNullOrBlank())
            {
                if (httpContext?.User.Identity is not ClaimsIdentity identity)
                    return;

                var userId = jwtService.ValidateJwtToken(token);

                if (!userId.IsNullOrBlank())
                {
                    // attach user to context on successful jwt validation

                    var user = await userRepository.GetUser(userId);
                    var roleMaps = await roleRepository.GetUserRoles(userId);
                    var roleIds = roleMaps.Select(r => r.RoleId).ToArray();
                    var claimMaps = await claimPermissionRepository.GetClaimsForRoleIds(roleIds);
                    var claims = await claimPermissionRepository.GetClaimsForClaimNames(claimMaps.Select(x => x.ClaimPermission).ToArray());

                    var requestUris = claims.Select(x => x.RequestUri.ToLower()).ToArray();

                    if (requestUris.Contains(requestUri.ToLower().Trim('/')))
                        httpContext.Items["User"] = user;
                }
            }

            // Call the next delegate/middleware in the pipeline.
            await _next(httpContext);
        }
    }
}
