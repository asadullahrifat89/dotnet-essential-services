using BaseModule.Domain.DTOs.Responses;
using MediatR;

namespace IdentityModule.Declarations.Commands
{
    public class ValidateTokenCommand : IRequest<ServiceResponse>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
