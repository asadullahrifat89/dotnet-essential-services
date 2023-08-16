using Base.Application.DTOs.Responses;
using MediatR;

namespace Identity.Application.Commands
{
    public class UpdateRoleCommand : IRequest<ServiceResponse>
    {
        public string RoleId { get; set; } = string.Empty;

        public string[] Claims { get; set; } = new string[] { };
    }
}
