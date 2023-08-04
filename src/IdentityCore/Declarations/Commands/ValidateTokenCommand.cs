using BaseCore.Models.Responses;
using MediatR;

namespace IdentityCore.Declarations.Commands
{
    public class ValidateTokenCommand : IRequest<ServiceResponse>
    {
        public string RefreshToken { get; set; } = string.Empty;
    }
}
