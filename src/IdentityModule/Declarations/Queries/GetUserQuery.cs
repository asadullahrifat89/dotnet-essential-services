using BaseModule.Models.Responses;
using MediatR;

namespace IdentityModule.Declarations.Queries
{
    public class GetUserQuery : IRequest<QueryRecordResponse<UserResponse>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
