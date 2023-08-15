using BaseModule.Domain.DTOs.Responses;
using MediatR;

namespace IdentityModule.Declarations.Commands
{
    public class UpdateRoleCommand : IRequest<ServiceResponse>
    {
        public string RoleId { get; set; } = string.Empty;

        public string[] Claims { get; set; } = new string[] { };
    }
}
