using BaseModule.Application.DTOs.Responses;
using IdentityModule.Domain.Entities;

namespace IdentityModule.Domain.Repositories.Interfaces
{
    public interface IRoleRepository
    {
        Task<bool> BeAnExistingRole(string role);

        Task<bool> BeAnExistingRoleById(string id);

        Task<ServiceResponse> AddRole(Role role, string[] claims);

        Task<ServiceResponse> UpdateRole(string roleId, string[] claims);

        Task<Role[]> GetRolesByNames(string[] names);

        Task<UserRoleMap[]> GetUserRoles(string userId);

        Task<QueryRecordsResponse<Role>> GetRoles(string searchTerm, int pageIndex, int pageSize);

        Task<QueryRecordsResponse<Role>> GetRolesByUserId(string userId);


    }
}
