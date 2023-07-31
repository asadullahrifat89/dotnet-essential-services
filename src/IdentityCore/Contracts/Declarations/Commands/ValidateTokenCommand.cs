using IdentityCore.Models.Responses;
using MediatR;

namespace IdentityCore.Contracts.Declarations.Commands
{
    public class ValidateTokenCommand : IRequest<ServiceResponse>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
