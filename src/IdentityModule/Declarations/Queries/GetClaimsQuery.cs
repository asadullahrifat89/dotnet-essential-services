using MediatR;
using BaseModule.Models.Responses;
using IdentityModule.Models.Entities;

namespace IdentityModule.Declarations.Queries
{
    public class GetClaimsQuery : IRequest<QueryRecordsResponse<ClaimPermission>>
    {

    }
}
