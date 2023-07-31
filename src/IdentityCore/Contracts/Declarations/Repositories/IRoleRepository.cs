using IdentityCore.Contracts.Declarations.Commands;
using IdentityCore.Models.Responses;

namespace IdentityCore.Contracts.Declarations.Repositories
{
    public interface IRoleRepository
    {
        Task<bool> BeAnExistingRole(string role);

        Task<ServiceResponse> AddRole(AddRoleCommand command);
    }
}
