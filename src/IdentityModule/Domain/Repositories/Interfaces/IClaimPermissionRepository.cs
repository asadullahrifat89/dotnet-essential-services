using BaseModule.Application.DTOs.Responses;
using IdentityModule.Application.Commands;
using IdentityModule.Application.Queries;
using IdentityModule.Domain.Entities;

namespace IdentityModule.Domain.Repositories.Interfaces
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
