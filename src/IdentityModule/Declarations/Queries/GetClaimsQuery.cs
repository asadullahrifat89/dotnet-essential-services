using MediatR;
using IdentityModule.Models.Entities;
using BaseModule.Domain.DTOs.Responses;

namespace IdentityModule.Declarations.Queries
{
    public class GetClaimsQuery : IRequest<QueryRecordsResponse<ClaimPermission>>
    {

    }
}
