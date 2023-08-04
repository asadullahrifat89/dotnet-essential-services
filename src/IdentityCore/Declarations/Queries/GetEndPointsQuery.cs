using IdentityCore.Models.Responses;
using MediatR;

namespace IdentityCore.Declarations.Queries
{
    public class GetEndPointsQuery : IRequest<QueryRecordsResponse<string>>
    {

    }
}
