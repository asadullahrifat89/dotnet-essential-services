using BaseModule.Application.DTOs.Responses;
using IdentityModule.Domain.Entities;
using MediatR;

namespace IdentityModule.Application.Queries
{
    public class GetUserRolesQuery : IRequest<QueryRecordsResponse<Role>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
