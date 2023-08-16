using Base.Application.DTOs.Responses;
using Identity.Application.DTOs;
using MediatR;

namespace Identity.Application.Queries
{
    public class GetUserQuery : IRequest<QueryRecordResponse<UserResponse>>
    {
        public string UserId { get; set; } = string.Empty;
    }
}
