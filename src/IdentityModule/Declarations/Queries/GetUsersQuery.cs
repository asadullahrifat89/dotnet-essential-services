using BaseModule.Application.DTOs.Requests;
using BaseModule.Application.DTOs.Responses;

namespace IdentityModule.Declarations.Queries
{
    public class GetUsersQuery : PagedRequestBase<UserResponse>
    {
        public string SearchTerm { get; set; } = string.Empty;
    }
}
