using BaseModule.Application.DTOs.Responses;
using BaseModule.Domain.Entities;
using MediatR;

namespace IdentityModule.Application.Commands
{
    public class CreateUserCommand : IRequest<ServiceResponse>
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string ProfileImageUrl { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public Address Address { get; set; } = new Address();

        public string[] Roles { get; set; } = new string[] { };

        public string[] MetaTags { get; set; } = new string[] { };

        //public string TenantId { get; set; } = string.Empty;
    }
}
