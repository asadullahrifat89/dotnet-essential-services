using Base.Infrastructure.Providers.Interfaces;
using Identity.Application.Providers.Interfaces;
using Identity.Domain.Entities;
using Identity.Domain.Repositories.Interfaces;
using MongoDB.Driver;

namespace Identity.Infrastructure.Persistence
{
    public class ClaimPermissionRepository : IClaimPermissionRepository
    {
        #region Fields

        private readonly IMongoDbContextProvider _mongoDbContextProvider;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;
        private readonly IRoleRepository _roleRepository;

        #endregion

        #region Ctor

        public ClaimPermissionRepository(IMongoDbContextProvider mongoDbService, IAuthenticationContextProvider authenticationContext, IRoleRepository roleRepository)
        {
            _mongoDbContextProvider = mongoDbService;
            _authenticationContextProvider = authenticationContext;
            _roleRepository = roleRepository;
        }
        #endregion

        #region Methods

        public async Task<bool> BeAnExistingClaimPermission(string claim)
        {
            var filter = Builders<ClaimPermission>.Filter.Where(x => x.Name.ToLower() == claim.ToLower());

            return await _mongoDbContextProvider.Exists(filter);
        }

        public async Task<RoleClaimPermissionMap[]> GetClaimsForRoleIds(string[] roleIds)
        {
            var filter = Builders<RoleClaimPermissionMap>.Filter.In(x => x.RoleId, roleIds);

            var results = await _mongoDbContextProvider.GetDocuments(filter: filter);

            return results is not null ? results.ToArray() : Array.Empty<RoleClaimPermissionMap>();
        }

        public async Task<ClaimPermission> AddClaimPermission(ClaimPermission claimPermission)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            await _mongoDbContextProvider.InsertDocument(claimPermission);

            return claimPermission;
        }

        public async Task<ClaimPermission[]> GetClaimsForClaimNames(string[] claimNames)
        {
            var filter = Builders<ClaimPermission>.Filter.In(x => x.Name, claimNames);

            var results = await _mongoDbContextProvider.GetDocuments(filter: filter);

            return results is not null ? results.ToArray() : Array.Empty<ClaimPermission>();
        }

        public async Task<(long Count, ClaimPermission[] ClaimPermissions)> GetClaims()
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var filter = Builders<ClaimPermission>.Filter.Empty;

            var count = await _mongoDbContextProvider.CountDocuments(filter: filter);

            var claims = await _mongoDbContextProvider.GetDocuments(filter: filter);

            return (count, claims is not null ? claims.ToArray() : Array.Empty<ClaimPermission>());
        }

        public async Task<ClaimPermission[]> GetUserClaims(string userId)
        {
            // find user roles, then from roles, find claims, then from claims assign in token

            var roles = await _roleRepository.GetUserRoles(userId);

            var roleIds = roles.Select(r => r.RoleId).Distinct().ToArray();

            var roleClaimMaps = await GetClaimsForRoleIds(roleIds);

            var claims = await GetClaimsForClaimNames(roleClaimMaps.Select(x => x.ClaimPermission).Distinct().ToArray());

            return claims;
        }

        #endregion
    }
}
