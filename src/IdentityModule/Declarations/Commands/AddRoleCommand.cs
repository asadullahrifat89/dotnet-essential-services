using BaseModule.Domain.DTOs.Responses;
using MediatR;

namespace IdentityModule.Declarations.Commands
{
    public class AddRoleCommand : IRequest<ServiceResponse>
    {
        public string Name { get; set; } = string.Empty;

        public string[] Claims { get; set; } = new string[0];
    }
}
