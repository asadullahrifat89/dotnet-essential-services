using Base.Application.DTOs.Responses;
using Identity.Domain.Entities;
using MediatR;

namespace Identity.Application.Queries
{
    public class GetUserRolesQuery : IRequest<QueryRecordsResponse<Role>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
