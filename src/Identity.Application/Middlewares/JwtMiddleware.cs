using Base.Application.Providers.Interfaces;
using Identity.Application.Services.Interfaces;
using Identity.Domain.Entities;
using Microsoft.AspNetCore.Http;
using MongoDB.Driver;
using System.Security.Claims;
using Base.Application.Extensions;

namespace Identity.Application.Middlewares
{
    public class JwtMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly IMongoDbContextProvider _mongoDbContextProvider;

        public JwtMiddleware(RequestDelegate next, IMongoDbContextProvider mongoDbService)
        {
            _next = next;
            _mongoDbContextProvider = mongoDbService;
        }

        public async Task Invoke(
            HttpContext httpContext,
            //IRoleRepository roleRepository,
            //IClaimPermissionRepository claimPermissionRepository,
            IJwtService jwtService)
        {
            var token = httpContext.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            var requestUri = httpContext.Request.Path.Value;

            httpContext.Items["AccessToken"] = token;
            httpContext.Items["RequestUri"] = requestUri;

            if (!token.IsNullOrBlank())
            {
                if (httpContext?.User.Identity is not ClaimsIdentity identity)
                    return;

                var userId = jwtService.ValidateJwtToken(token);

                if (!userId.IsNullOrBlank())
                {
                    // attach user to context on successful jwt validation

                    var user = await GetUser(userId);

                    if (user is not null)
                    {
                        // TODO: do role based authentication later

                        //var roleMaps = await roleRepository.GetUserRoles(userId);
                        //var roleIds = roleMaps.Select(r => r.RoleId).ToArray();
                        //var claimMaps = await claimPermissionRepository.GetClaimsForRoleIds(roleIds);
                        //var claims = await claimPermissionRepository.GetClaimsForClaimNames(claimMaps.Select(x => x.ClaimPermission).ToArray());

                        //var requestUris = claims.Select(x => x.RequestUri.ToLower()).ToArray();

                        //if (requestUris.Contains(requestUri.ToLower().Trim('/')))
                        //{
                        httpContext.Items["User"] = user;
                        //} 
                    }
                }
            }

            // Call the next delegate/middleware in the pipeline.
            await _next(httpContext);
        }

        private async Task<User> GetUser(string userId)
        {
            var filter = Builders<User>.Filter.Eq(x => x.Id, userId);

            return await _mongoDbContextProvider.FindOne(filter);
        }
    }
}
