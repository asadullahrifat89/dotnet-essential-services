using BaseModule.Application.DTOs.Responses;
using IdentityModule.Domain.Entities;
using MediatR;

namespace IdentityModule.Application.Queries
{
    public class GetRolesQuery : IRequest<QueryRecordsResponse<Role>>
    {
    }
}
