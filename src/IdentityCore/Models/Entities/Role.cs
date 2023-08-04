using IdentityCore.Declarations.Commands;
using BaseCore.Extensions;
using BaseCore.Models.Entities;

namespace IdentityCore.Models.Entities
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
