using IdentityCore.Models.Entities;

namespace IdentityCore.Contracts.Declarations.Repositories
{
    public interface IClaimPermissionRepository
    {
        bool BeAnExistingClaimPermission(string claim);

        Task<RoleClaimPermissionMap[]> GetClaimsForRoleIds(string[] roleIds);
    }
}
