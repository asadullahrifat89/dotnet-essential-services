using BaseModule.Application.DTOs.Requests;
using IdentityModule.Application.DTOs;

namespace IdentityModule.Application.Queries
{
    public class GetUsersQuery : PagedRequestBase<UserResponse>
    {
        public string SearchTerm { get; set; } = string.Empty;
    }
}
