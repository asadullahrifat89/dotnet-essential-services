using BaseModule.Application.DTOs.Responses;
using IdentityModule.Domain.Entities;
using IdentityModule.Infrastructure.Extensions;
using MediatR;

namespace IdentityModule.Application.Commands
{
    public class AddRoleCommand : IRequest<ServiceResponse>
    {
        public string Name { get; set; } = string.Empty;

        public string[] Claims { get; set; } = new string[0];

        public static Role Initialize(AddRoleCommand command, AuthenticationContext authenticationContext)
        {
            return new Role()
            {
                Name = command.Name,
                TimeStamp = authenticationContext.BuildCreatedByTimeStamp(),
                //TenantId = authenticationContext.TenantId,
            };
        }
    }
}
