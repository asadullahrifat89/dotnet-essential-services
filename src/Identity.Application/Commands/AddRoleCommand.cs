using Base.Application.DTOs.Responses;
using Identity.Application.Extensions;
using Identity.Domain.Entities;
using MediatR;

namespace Identity.Application.Commands
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
