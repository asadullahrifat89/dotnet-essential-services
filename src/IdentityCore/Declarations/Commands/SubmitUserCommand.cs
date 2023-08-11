using BaseCore.Models.Responses;
using MediatR;

namespace IdentityCore.Declarations.Commands
{
    public class SubmitUserCommand: IRequest<ServiceResponse>
    {
        public string Email { get; set; } = string.Empty;

        public string[] MetaTags { get; set; } = new string[] { };
    }
}
