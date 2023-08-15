using BaseModule.Application.DTOs.Responses;
using IdentityModule.Domain.Entities;
using IdentityModule.Infrastructure.Extensions;
using MediatR;

namespace IdentityModule.Application.Commands
{
    public class SubmitUserCommand : IRequest<ServiceResponse>
    {
        public string Email { get; set; } = string.Empty;

        public string[] MetaTags { get; set; } = new string[] { };

        public static User Initialize(SubmitUserCommand command, AuthenticationContext authenticationContext)
        {
            var user = new User()
            {
                Email = command.Email,
                MetaTags = command.MetaTags,
                UserStatus = UserStatus.Inactive,
                TimeStamp = authenticationContext.BuildCreatedByTimeStamp(),
                //TenantId = command.TenantId,
            };

            return user;
        }
    }
}
