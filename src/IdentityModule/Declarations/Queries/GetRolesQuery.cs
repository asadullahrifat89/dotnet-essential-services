using BaseModule.Domain.DTOs.Responses;
using IdentityModule.Models.Entities;
using MediatR;

namespace IdentityModule.Declarations.Queries
{
    public class GetRolesQuery : IRequest<QueryRecordsResponse<Role>>
    {
    }
}
