using Base.Application.DTOs.Responses;
using MediatR;

namespace Identity.Application.Commands
{
    public class UpdateUserPasswordCommand : IRequest<ServiceResponse>
    {
        public string UserId { get; set; } = string.Empty;

        public string OldPassword { get; set; } = string.Empty;

        public string NewPassword { get; set; } = string.Empty;
    }
}
