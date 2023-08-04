using IdentityCore.Models.Entities;
using IdentityCore.Models.Responses;
using MediatR;

namespace IdentityCore.Declarations.Queries
{
    public class GetRolesQuery : IRequest<QueryRecordsResponse<Role>>
    {
    }
}
