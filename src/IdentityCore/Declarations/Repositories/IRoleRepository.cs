using BaseCore.Models.Responses;
using IdentityCore.Declarations.Commands;
using IdentityCore.Declarations.Queries;
using IdentityCore.Models.Entities;

namespace IdentityCore.Declarations.Repositories
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
