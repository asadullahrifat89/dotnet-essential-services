using BaseModule.Application.DTOs.Responses;
using BaseModule.Domain.Entities;
using BaseModule.Infrastructure.Extensions;
using IdentityModule.Domain.Entities;
using MediatR;

namespace IdentityModule.Application.Commands
{
    public class UpdateUserCommand : IRequest<ServiceResponse>
    {
        public string UserId { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string ProfileImageUrl { get; set; } = string.Empty;

        public Address Address { get; set; } = new Address();

        public static User Initialize(UpdateUserCommand command, AuthenticationContext authenticationContext)
        {
            var user = new User()
            {
                Id = command.UserId,
                FirstName = command.FirstName,
                LastName = command.LastName,
                DisplayName = (command.FirstName + " " + command.LastName).Trim(),
                ProfileImageUrl = command.ProfileImageUrl,
                Address = command.Address,
                UserStatus = UserStatus.Inactive,
                //TenantId = command.TenantId,
            };

            return user;
        }
    }
}
