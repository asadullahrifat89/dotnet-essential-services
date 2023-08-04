using BaseCore.Models.Responses;
using IdentityCore.Models.Entities;
using MediatR;

namespace IdentityCore.Declarations.Queries
{
    public class GetRolesQuery : IRequest<QueryRecordsResponse<Role>>
    {
    }
}
