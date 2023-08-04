using BaseCore.Models.Responses;
using IdentityCore.Declarations.Commands;
using IdentityCore.Declarations.Queries;
using IdentityCore.Models.Entities;

namespace IdentityCore.Declarations.Repositories
{
    public interface IClaimPermissionRepository
    {
        Task<bool> BeAnExistingClaimPermission(string claim);

        Task<RoleClaimPermissionMap[]> GetClaimsForRoleIds(string[] roleIds);

        Task<ClaimPermission[]> GetClaimsForClaimNames(string[] claimNames);

        Task<ServiceResponse> AddClaimPermission(AddClaimPermissionCommand command);

        Task<QueryRecordsResponse<ClaimPermission>> GetClaims(GetClaimsQuery query);

        Task<ClaimPermission[]> GetUserClaims(string userId);
    }
}
