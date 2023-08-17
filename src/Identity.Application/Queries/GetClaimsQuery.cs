using Base.Application.DTOs.Responses;
using Identity.Domain.Entities;
using MediatR;

namespace Identity.Application.Queries
{
    public class GetClaimsQuery : IRequest<QueryRecordsResponse<ClaimPermission>>
    {

    }
}
