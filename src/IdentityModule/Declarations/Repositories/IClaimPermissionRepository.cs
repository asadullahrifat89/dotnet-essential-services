using BaseModule.Models.Responses;
using IdentityModule.Declarations.Commands;
using IdentityModule.Declarations.Queries;
using IdentityModule.Models.Entities;

namespace IdentityModule.Declarations.Repositories
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
