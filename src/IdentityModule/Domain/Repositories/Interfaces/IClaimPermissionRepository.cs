using BaseModule.Application.DTOs.Responses;
using IdentityModule.Domain.Entities;

namespace IdentityModule.Domain.Repositories.Interfaces
{
    public interface IClaimPermissionRepository
    {
        Task<bool> BeAnExistingClaimPermission(string claim);

        Task<RoleClaimPermissionMap[]> GetClaimsForRoleIds(string[] roleIds);

        Task<ClaimPermission[]> GetClaimsForClaimNames(string[] claimNames);

        Task<ServiceResponse> AddClaimPermission(ClaimPermission claimPermission);

        Task<QueryRecordsResponse<ClaimPermission>> GetClaims();

        Task<ClaimPermission[]> GetUserClaims(string userId);
    }
}
