using BaseModule.Application.DTOs.Responses;
using IdentityModule.Application.DTOs;
using MediatR;

namespace IdentityModule.Application.Queries
{
    public class GetUserQuery : IRequest<QueryRecordResponse<UserResponse>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
