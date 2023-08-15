using BaseCommon;
using BaseCore.Extensions;
using BaseCore.Models.Responses;
using BaseCore.Services;
using IdentityCore.Declarations.Queries;
using IdentityCore.Declarations.Repositories;

namespace IdentityCore.Implementations.Repositories
{
    public class EndPointRepository : IEndpointsRepository
    {
        private readonly IAuthenticationContextProvider _authenticationContext;

        public EndPointRepository(IAuthenticationContextProvider authenticationContext)
        {
            _authenticationContext = authenticationContext;
        }

        public Task<QueryRecordsResponse<string>> GetEndpointList(GetEndPointsQuery query)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var endpoints = GetEndpointRoutes();

            return Task.FromResult(Response.BuildQueryRecordsResponse<string>().BuildSuccessResponse(count: endpoints.Length, records: endpoints, requestUri: authCtx?.RequestUri));
        }

        public static string[] GetEndpointRoutes()
        {
            var endpoints = ClassExtensions.GetConstants(typeof(EndpointRoutes)).Select(x => x.GetValue(x.Name).ToString().ToLower()).ToArray();

            return endpoints;
        }
    }
}
