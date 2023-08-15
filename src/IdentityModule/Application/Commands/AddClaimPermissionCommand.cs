using BaseModule.Application.DTOs.Responses;
using IdentityModule.Domain.Entities;
using IdentityModule.Infrastructure.Extensions;
using MediatR;

namespace IdentityModule.Application.Commands
{
    public class AddClaimPermissionCommand : IRequest<ServiceResponse>
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
