using IdentityCore.Models.Responses;
using MediatR;

namespace IdentityCore.Declarations.Queries
{
    public class GetUserQuery : IRequest<QueryRecordResponse<UserResponse>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
