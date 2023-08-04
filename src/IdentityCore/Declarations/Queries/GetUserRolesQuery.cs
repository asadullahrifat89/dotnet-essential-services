using IdentityCore.Models.Entities;
using IdentityCore.Models.Responses;
using MediatR;

namespace IdentityCore.Declarations.Queries
{
    public class GetUserRolesQuery : IRequest<QueryRecordsResponse<Role>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
