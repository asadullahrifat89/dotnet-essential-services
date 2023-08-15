using MediatR;
using IdentityModule.Models.Entities;
using BaseModule.Application.DTOs.Responses;

namespace IdentityModule.Declarations.Queries
{
    public class GetClaimsQuery : IRequest<QueryRecordsResponse<ClaimPermission>>
    {

    }
}
