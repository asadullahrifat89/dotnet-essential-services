using Base.Application.DTOs.Requests;
using Identity.Application.DTOs;

namespace Identity.Application.Queries
{
    public class GetUsersQuery : PagedRequestBase<UserResponse>
    {
        public string SearchTerm { get; set; } = string.Empty;
    }
}
