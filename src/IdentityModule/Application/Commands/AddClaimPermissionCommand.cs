using BaseModule.Application.DTOs.Responses;
using MediatR;

namespace IdentityModule.Application.Commands
{
    public class AddClaimPermissionCommand : IRequest<ServiceResponse>
    {
        public string Name { get; set; } = string.Empty;

        public string RequestUri { get; set; } = string.Empty;
    }
}
