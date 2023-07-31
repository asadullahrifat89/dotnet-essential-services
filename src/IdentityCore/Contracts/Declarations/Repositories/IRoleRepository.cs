using IdentityCore.Contracts.Declarations.Commands;
using IdentityCore.Models.Entities;
using IdentityCore.Models.Responses;

namespace IdentityCore.Contracts.Declarations.Repositories
{
    public interface IRoleRepository
    {
        Task<bool> BeAnExistingRole(string role);

        Task<ServiceResponse> AddRole(AddRoleCommand command);

        Task<Role[]> GetRolesByNames(string[] names);

        Task<UserRoleMap[]> GetUserRoles(string userId);
    }
}
