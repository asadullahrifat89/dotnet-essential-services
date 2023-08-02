using IdentityCore.Contracts.Declarations.Commands;
using IdentityCore.Models.Entities;
using IdentityCore.Models.Responses;

namespace IdentityCore.Contracts.Declarations.Repositories
{
    public interface IClaimPermissionRepository
    {
        Task<bool> BeAnExistingClaimPermission(string claim);

        Task<RoleClaimPermissionMap[]> GetClaimsForRoleIds(string[] roleIds);

        Task<ClaimPermission[]> GetClaimsForClaimNames(string[] claimNames);

        Task<ServiceResponse> AddClaimPermission(AddClaimPermissionCommand command);
    }
}
