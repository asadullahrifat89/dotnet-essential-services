using BaseModule.Application.DTOs.Responses;
using MediatR;

namespace IdentityModule.Declarations.Commands
{
    public class SubmitUserCommand : IRequest<ServiceResponse>
    {
        public string Email { get; set; } = string.Empty;

        public string[] MetaTags { get; set; } = new string[] { };
    }
}
