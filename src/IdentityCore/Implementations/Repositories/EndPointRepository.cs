using BaseCore.Declarations.Services;
using BaseCore.Models.Responses;
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

            var endpoints = EndpointRoutes.GetEndpointRoutes();

            return Task.FromResult(Response.BuildQueryRecordsResponse<string>().BuildSuccessResponse(count: endpoints.Length, records: endpoints, requestUri: authCtx?.RequestUri));
        }
    }
}
