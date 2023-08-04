using IdentityCore.Contracts.Declarations.Queries;
using IdentityCore.Contracts.Declarations.Repositories;
using IdentityCore.Contracts.Declarations.Services;
using IdentityCore.Models.Entities;
using IdentityCore.Models.Responses;

namespace IdentityCore.Contracts.Implementations.Repositories
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
