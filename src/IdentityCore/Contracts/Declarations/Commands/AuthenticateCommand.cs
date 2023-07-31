using IdentityCore.Models.Responses;
using MediatR;

namespace IdentityCore.Contracts.Declarations.Commands
{
    public class AuthenticateCommand : IRequest<ServiceResponse>
    {
        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;
    }
}
