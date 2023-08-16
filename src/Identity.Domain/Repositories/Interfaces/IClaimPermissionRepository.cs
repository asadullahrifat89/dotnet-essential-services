using Identity.Domain.Entities;

namespace Identity.Domain.Repositories.Interfaces
{
    public interface IClaimPermissionRepository
    {
        Task<bool> BeAnExistingClaimPermission(string claim);

        Task<RoleClaimPermissionMap[]> GetClaimsForRoleIds(string[] roleIds);

        Task<ClaimPermission[]> GetClaimsForClaimNames(string[] claimNames);

        Task<ClaimPermission> AddClaimPermission(ClaimPermission claimPermission);

        Task<(long Count, ClaimPermission[] ClaimPermissions)> GetClaims();

        Task<ClaimPermission[]> GetUserClaims(string userId);
    }
}
