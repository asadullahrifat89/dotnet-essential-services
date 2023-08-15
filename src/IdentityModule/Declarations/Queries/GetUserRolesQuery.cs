using BaseModule.Application.DTOs.Responses;
using IdentityModule.Models.Entities;
using MediatR;

namespace IdentityModule.Declarations.Queries
{
    public class GetUserRolesQuery : IRequest<QueryRecordsResponse<Role>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
