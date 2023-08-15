using BaseModule.Application.DTOs.Responses;
using IdentityModule.Domain.Entities;
using MediatR;

namespace IdentityModule.Application.Commands
{
    public class UpdateRoleCommand : IRequest<ServiceResponse>
    {
        public string RoleId { get; set; } = string.Empty;

        public string[] Claims { get; set; } = new string[] { };      
    }
}
