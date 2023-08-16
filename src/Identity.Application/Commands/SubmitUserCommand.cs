using Base.Application.DTOs.Responses;
using Identity.Application.Extensions;
using Identity.Domain.Entities;
using MediatR;

namespace Identity.Application.Commands
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
