using BaseCore.Models.Responses;
using MediatR;

namespace IdentityCore.Declarations.Commands
{
    public class AuthenticateCommand : IRequest<ServiceResponse>
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
