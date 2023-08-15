using BaseModule.Application.DTOs.Responses;
using MediatR;

namespace IdentityModule.Declarations.Queries
{
    public class GetUserQuery : IRequest<QueryRecordResponse<UserResponse>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
