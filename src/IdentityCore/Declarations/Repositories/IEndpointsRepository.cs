using BaseCore.Models.Responses;
using IdentityCore.Declarations.Queries;

namespace IdentityCore.Declarations.Repositories
{
    public interface IEndpointsRepository
    {
        Task<QueryRecordsResponse<string>> GetEndpointList(GetEndPointsQuery query);
    }
}
