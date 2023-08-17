using Base.Application.DTOs.Responses;
using MediatR;

namespace Identity.Application.Commands
{
    public class VerifyUserAccountActivationRequestCommand : IRequest<ServiceResponse>
    {
        public string Email { get; set; } = string.Empty;

        public string ActivationKey { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
