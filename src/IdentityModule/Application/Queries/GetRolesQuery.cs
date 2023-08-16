using BaseModule.Application.DTOs.Requests;
using IdentityModule.Domain.Entities;

namespace IdentityModule.Application.Queries
{
    public class GetRolesQuery : PagedRequestBase<Role>
    {
        public string SearchTerm { get; set; } = string.Empty;
    }
}
