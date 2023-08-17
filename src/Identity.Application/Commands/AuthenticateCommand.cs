using Base.Application.DTOs.Responses;
using MediatR;

namespace Identity.Application.Commands
{
    public class AuthenticateCommand : IRequest<ServiceResponse>
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
