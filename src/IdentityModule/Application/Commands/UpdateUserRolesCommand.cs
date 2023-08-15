using BaseModule.Application.DTOs.Responses;
using MediatR;

namespace IdentityModule.Application.Commands
{
    public class UpdateUserRolesCommand : IRequest<ServiceResponse>
    {
        public string UserId { get; set; } = string.Empty;

        public string[] RoleNames { get; set; } = Array.Empty<string>();
    }
}
