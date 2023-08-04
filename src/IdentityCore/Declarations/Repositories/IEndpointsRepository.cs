using IdentityCore.Declarations.Queries;
using IdentityCore.Models.Responses;

namespace IdentityCore.Declarations.Repositories
{
    public interface IEndpointsRepository
    {
        Task<QueryRecordsResponse<string>> GetEndpointList(GetEndPointsQuery query);
    }
}
