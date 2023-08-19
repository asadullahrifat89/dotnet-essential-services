using Base.Application.DTOs.Responses;
using Base.Domain.Entities;
using Identity.Domain.Entities;
using MediatR;

namespace Identity.Application.Commands
{
    public class UpdateUserCommand : IRequest<ServiceResponse>
    {
        public string UserId { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string ProfileImageUrl { get; set; } = string.Empty;

        public Address[] Addresses { get; set; } = new Address[] { };

        public static User Initialize(UpdateUserCommand command, AuthenticationContext authenticationContext)
        {
            var user = new User()
            {
                Id = command.UserId,
                FirstName = command.FirstName,
                LastName = command.LastName,
                DisplayName = (command.FirstName + " " + command.LastName).Trim(),
                ProfileImageUrl = command.ProfileImageUrl,
                Addresses = command.Addresses,
                UserStatus = UserStatus.Inactive,
                //TenantId = command.TenantId,
            };

            return user;
        }
    }
}
