using Identity.Domain.Entities;

namespace Identity.Domain.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<bool> BeAnExistingRole(string role);

        Task<bool> BeAnExistingRoleById(string id);

        Task<Role> AddRole(Role role, string[] claims);

        Task<Role> UpdateRole(string roleId, string[] claims);

        Task<Role[]> GetRolesByNames(string[] names);

        Task<UserRoleMap[]> GetUserRoles(string userId);

        Task<(long Count, Role[] Roles)> GetRoles(string searchTerm, int pageIndex, int pageSize);

        Task<(long Count, Role[] Roles)> GetRolesByUserId(string userId);
    }
}
