using BaseModule.Domain.Entities;
using IdentityModule.Application.Commands;
using IdentityModule.Infrastructure.Extensions;

namespace IdentityModule.Domain.Entities
{
    public class ClaimPermission : EntityBase
    {
        public string Name { get; set; } = string.Empty;

        public string RequestUri { get; set; } = string.Empty;

        public static ClaimPermission Initialize(AddClaimPermissionCommand command, AuthenticationContext authenticationContext)
        {
            return new ClaimPermission()
            {
                Name = command.Name,
                RequestUri = command.RequestUri,
                TimeStamp = authenticationContext.BuildCreatedByTimeStamp(),
                //TenantId = authenticationContext.TenantId,
            };
        }
    }
}
