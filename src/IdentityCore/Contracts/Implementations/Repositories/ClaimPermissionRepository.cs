using IdentityCore.Contracts.Declarations.Commands;
using IdentityCore.Contracts.Declarations.Queries;
using IdentityCore.Contracts.Declarations.Repositories;
using IdentityCore.Contracts.Declarations.Services;
using IdentityCore.Models.Entities;
using IdentityCore.Models.Responses;
using MongoDB.Driver;

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
        #endregion

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
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var claimPermission = ClaimPermission.Initialize(command, authCtx);

            await _mongoDbService.InsertDocument(claimPermission);

            return Response.BuildServiceResponse().BuildSuccessResponse(claimPermission, authCtx?.RequestUri);
        }

        public async Task<ClaimPermission[]> GetClaimsForClaimNames(string[] claimNames)
        {
            var filter = Builders<ClaimPermission>.Filter.In(x => x.Name, claimNames);

            var results = await _mongoDbService.GetDocuments(filter: filter);

            return results is not null ? results.ToArray() : Array.Empty<ClaimPermission>();
        }

        public async Task<QueryRecordsResponse<ClaimPermission>> GetClaims(GetClaimsQuery query)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var filter = Builders<ClaimPermission>.Filter.Empty;

            var count = await _mongoDbService.CountDocuments(filter: filter);

            var clams = await _mongoDbService.GetDocuments(filter: filter);

            return Response.BuildQueryRecordsResponse<ClaimPermission>().BuildSuccessResponse(
               count: count,
               records: clams is not null ? clams.ToArray() : Array.Empty<ClaimPermission>(), requestUri: authCtx?.RequestUri);
        }

        #endregion
    }
}
