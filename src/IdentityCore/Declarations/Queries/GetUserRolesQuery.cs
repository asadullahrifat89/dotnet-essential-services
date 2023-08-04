using BaseCore.Models.Responses;
using IdentityCore.Models.Entities;
using MediatR;

namespace IdentityCore.Declarations.Queries
{
    public class GetUserRolesQuery : IRequest<QueryRecordsResponse<Role>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
