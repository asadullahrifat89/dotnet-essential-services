using Base.Application.DTOs.Responses;
using Base.Domain.Entities;
using Identity.Application.Extensions;
using Identity.Domain.Entities;
using MediatR;
using Base.Application.Extensions;

namespace Identity.Application.Commands
{
    public class CreateUserCommand : IRequest<ServiceResponse>
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string ProfileImageUrl { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public Address[] Addresses { get; set; } = new Address[] { };

        public string[] MetaTags { get; set; } = new string[] { };

        public string[] Roles { get; set; } = new string[] { };

        //public string TenantId { get; set; } = string.Empty;

        public static User Initialize(CreateUserCommand command, AuthenticationContext authenticationContext)
        {
            var user = new User()
            {
                FirstName = command.FirstName,
                LastName = command.LastName,
                DisplayName = (command.FirstName + " " + command.LastName).Trim(),
                ProfileImageUrl = command.ProfileImageUrl,
                PhoneNumber = command.PhoneNumber,
                Email = command.Email,
                Password = command.Password.Encrypt(),
                Addresses = command.Addresses,
                MetaTags = command.MetaTags,
                UserStatus = UserStatus.Inactive,
                TimeStamp = authenticationContext.BuildCreatedByTimeStamp(),
                //TenantId = command.TenantId,
            };

            return user;
        }
    }
}
