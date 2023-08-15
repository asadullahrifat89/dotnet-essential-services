using MediatR;
using BaseModule.Application.DTOs.Responses;
using IdentityModule.Domain.Entities;

namespace IdentityModule.Application.Queries
{
    public class GetClaimsQuery : IRequest<QueryRecordsResponse<ClaimPermission>>
    {

    }
}
