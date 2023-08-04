using IdentityCore.Models.Responses;
using MediatR;

namespace IdentityCore.Declarations.Commands
{
    public class UpdateRoleCommand : IRequest<ServiceResponse>
    {
        public string RoleId { get; set; } = string.Empty;

        public string[] Claims { get; set; } = new string[] { };
    }
}
