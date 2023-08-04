using BaseCore.Models.Responses;
using MediatR;

namespace IdentityCore.Declarations.Commands
{
    public class UpdateUserRolesCommand : IRequest<ServiceResponse>
    {
        public string UserId { get; set; } = string.Empty;

        public string[] RoleNames { get; set; } = Array.Empty<string>();
    }
}
