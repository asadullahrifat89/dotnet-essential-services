using BaseModule.Domain.DTOs.Requests;
using BaseModule.Domain.DTOs.Responses;

namespace IdentityModule.Declarations.Queries
{
    public class GetUsersQuery : PagedRequestBase<UserResponse>
    {
        public string SearchTerm { get; set; } = string.Empty;
    }
}
