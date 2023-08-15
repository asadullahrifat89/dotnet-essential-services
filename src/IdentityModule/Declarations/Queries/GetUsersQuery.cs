using BaseModule.Models.Requests;
using BaseModule.Models.Responses;

namespace IdentityModule.Declarations.Queries
{
    public class GetUsersQuery : PagedRequestBase<UserResponse>
    {
        public string SearchTerm { get; set; } = string.Empty;
    }
}
