using BaseCore.Models.Responses;
using MediatR;

namespace IdentityCore.Declarations.Commands
{
    public class AddRoleCommand : IRequest<ServiceResponse>
    {
        public string Name { get; set; } = string.Empty;

        public string[] Claims { get; set; } = new string[0];
    }
}
