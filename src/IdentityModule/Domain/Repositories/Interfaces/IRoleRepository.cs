using BaseModule.Application.DTOs.Responses;
using IdentityModule.Application.Commands;
using IdentityModule.Application.Queries;
using IdentityModule.Domain.Entities;

namespace IdentityModule.Domain.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<bool> BeAnExistingRole(string role);

        Task<bool> BeAnExistingRoleById(string id);

        Task<ServiceResponse> AddRole(AddRoleCommand command);

        Task<ServiceResponse> UpdateRole(UpdateRoleCommand command);

        Task<Role[]> GetRolesByNames(string[] names);

        Task<UserRoleMap[]> GetUserRoles(string userId);

        Task<QueryRecordsResponse<Role>> GetRoles(GetRolesQuery query);

        Task<QueryRecordsResponse<Role>> GetRolesByUserId(GetUserRolesQuery query);


    }
}
