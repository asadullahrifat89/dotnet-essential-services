using Base.Application.DTOs.Responses;
using MediatR;

namespace Identity.Application.Commands
{
    public class UpdateUserRolesCommand : IRequest<ServiceResponse>
    {
        public string UserId { get; set; } = string.Empty;

        public string[] RoleNames { get; set; } = Array.Empty<string>();
    }
}
