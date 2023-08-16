using Base.Application.DTOs.Requests;
using Identity.Domain.Entities;

namespace Identity.Application.Queries
{
    public class GetRolesQuery : PagedRequestBase<Role>
    {
        public string SearchTerm { get; set; } = string.Empty;
    }
}
