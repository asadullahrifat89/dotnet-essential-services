using BaseModule.Application.DTOs.Responses;
using BaseModule.Domain.Entities;
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
    }
}
