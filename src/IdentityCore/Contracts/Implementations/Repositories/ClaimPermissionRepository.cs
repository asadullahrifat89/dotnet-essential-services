using IdentityCore.Contracts.Declarations.Commands;
using IdentityCore.Contracts.Declarations.Repositories;
using IdentityCore.Contracts.Declarations.Services;
using IdentityCore.Models.Entities;
using IdentityCore.Models.Responses;
using MongoDB.Driver;
using System.Data;

namespace IdentityCore.Contracts.Implementations.Repositories
{
    public class ClaimPermissionRepository : IClaimPermissionRepository
    {
        #region Fields

        private readonly IMongoDbService _mongoDbService;
        private readonly IAuthenticationContextProvider _authenticationContext;

        #endregion

        #region Ctor

        public ClaimPermissionRepository(IMongoDbService mongoDbService, IAuthenticationContextProvider authenticationContext)
        {
            _mongoDbService = mongoDbService;
            _authenticationContext = authenticationContext;
        }

        #region Methods

        public async Task<bool> BeAnExistingClaimPermission(string claim)
        {
            var filter = Builders<ClaimPermission>.Filter.Where(x => x.Name.ToLower() == claim.ToLower());

            return await _mongoDbService.Exists(filter);
        }

        public async Task<RoleClaimPermissionMap[]> GetClaimsForRoleIds(string[] roleIds)
        {
            var filter = Builders<RoleClaimPermissionMap>.Filter.In(x => x.RoleId, roleIds);

            var results = await _mongoDbService.GetDocuments(filter: filter);

            return results is not null ? results.ToArray() : Array.Empty<RoleClaimPermissionMap>();
        }

        public async Task<ServiceResponse> AddClaimPermission(AddClaimPermissionCommand command)
        {
            var claimPermission = ClaimPermission.Initialize(command, _authenticationContext.GetAuthenticationContext());

            await _mongoDbService.InsertDocument(claimPermission);

            return Response.BuildServiceResponse().BuildSuccessResponse(claimPermission);
        }

        #endregion

        #endregion
    }
}
