using BaseModule.Application.DTOs.Responses;
using MediatR;

namespace IdentityModule.Application.Commands
{
    public class AddRoleCommand : IRequest<ServiceResponse>
    {
        public string Name { get; set; } = string.Empty;

        public string[] Claims { get; set; } = new string[0];
    }
}
