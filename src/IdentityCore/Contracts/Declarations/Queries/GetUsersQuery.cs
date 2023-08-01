using IdentityCore.Models.Requests;
using IdentityCore.Models.Responses;

namespace IdentityCore.Contracts.Declarations.Queries
{
    public class GetUsersQuery : PagedRequestBase<UserResponse>
    {
        public string SearchTerm { get; set; } = string.Empty;
    }
}
