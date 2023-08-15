using BaseModule.Domain.DTOs.Responses;
using MediatR;


namespace IdentityModule.Declarations.Commands
{
    public class SendUserAccountActivationRequestCommand : IRequest<ServiceResponse>
    {
        public string Email { get; set; } = string.Empty;
    }
}
