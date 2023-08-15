using BaseModule.Models.Responses;
using IdentityModule.Declarations.Commands;
using IdentityModule.Declarations.Queries;
using IdentityModule.Models.Entities;

namespace IdentityModule.Declarations.Repositories
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
