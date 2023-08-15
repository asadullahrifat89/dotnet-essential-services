using BaseModule.Models.Responses;
using MediatR;

namespace IdentityModule.Declarations.Commands
{
    public class AddClaimPermissionCommand : IRequest<ServiceResponse>
    {
        public string Name { get; set; } = string.Empty;

        public string RequestUri { get; set; } = string.Empty;
    }
}
