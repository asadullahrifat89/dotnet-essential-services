using BaseModule.Application.DTOs.Requests;
using BaseModule.Application.DTOs.Responses;
using IdentityModule.Domain.Entities;
using MediatR;

namespace IdentityModule.Application.Queries
{
    public class GetRolesQuery : PagedRequestBase<Role>
    {
        public string SearchTerm { get; set; } = string.Empty;
    }
}
