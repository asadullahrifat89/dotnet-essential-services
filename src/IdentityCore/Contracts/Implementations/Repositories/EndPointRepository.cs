using IdentityCore.Contracts.Declarations.Queries;
using IdentityCore.Contracts.Declarations.Repositories;
using IdentityCore.Models.Responses;

namespace IdentityCore.Contracts.Implementations.Repositories
{
    public class EndPointRepository : IEndpointsRepository
    {
        public EndPointRepository()
        {

        }

        public Task<QueryRecordsResponse<string>> GetEndpointList(GetEndPointsQuery query)
        {
            var endpoints = EndpointRoutes.GetEndpointRoutes();

            return Task.FromResult(Response.BuildQueryRecordsResponse<string>().BuildSuccessResponse(count: endpoints.Length, records: endpoints));
        }
    }
}
