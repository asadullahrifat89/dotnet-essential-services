using BaseCore.Models.Responses;
using MediatR;
using IdentityCore.Models.Entities;

namespace IdentityCore.Declarations.Queries
{
    public class GetClaimsQuery : IRequest<QueryRecordsResponse<ClaimPermission>>
    {

    }
}
