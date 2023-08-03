using IdentityCore.Contracts.Declarations.Queries;
using IdentityCore.Models.Responses;

namespace IdentityCore.Contracts.Declarations.Repositories
{
    public interface IEndpointsRepository
    {
        Task<QueryRecordsResponse<string>> GetEndpointList(GetEndPointsQuery query);
    }
}
