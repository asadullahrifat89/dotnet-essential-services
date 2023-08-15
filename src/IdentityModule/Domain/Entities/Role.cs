using BaseModule.Domain.Entities;
using IdentityModule.Application.Commands;
using IdentityModule.Infrastructure.Extensions;

namespace IdentityModule.Domain.Entities
{
    public class Role : EntityBase
    {
        public string Name { get; set; } = string.Empty;

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
