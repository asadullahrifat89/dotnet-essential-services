using IdentityCore.Contracts.Declarations.Repositories;
using IdentityCore.Contracts.Declarations.Services;
using IdentityCore.Models.Entities;
using MongoDB.Driver;

namespace IdentityCore.Contracts.Implementations.Repositories
{
    public class ClaimPermissionRepository : IClaimPermissionRepository
    {
        #region Fields

        private readonly IMongoDbService _mongoDbService;

        #endregion

        #region Ctor

        public ClaimPermissionRepository(IMongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        #region Methods

        public bool BeAnExistingClaimPermission(string claim)
        {
            return Constants.Claims.Contains(claim);
        }

        public async Task<RoleClaimPermissionMap[]> GetClaimsForRoleIds(string[] roleIds)
        {
            var filter = Builders<RoleClaimPermissionMap>.Filter.In(x => x.RoleId, roleIds);

            var results = await _mongoDbService.GetDocuments(filter: filter);

            return results is not null ? results.ToArray() : Array.Empty<RoleClaimPermissionMap>();
        }

        #endregion

        #endregion
    }
}
