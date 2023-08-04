using BaseCore.Models.Requests;
using BaseCore.Models.Responses;

namespace IdentityCore.Declarations.Queries
{
    public class GetUsersQuery : PagedRequestBase<UserResponse>
    {
        public string SearchTerm { get; set; } = string.Empty;
    }
}
