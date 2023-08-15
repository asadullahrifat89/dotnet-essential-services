using BaseModule.Application.DTOs.Responses;
using BaseModule.Application.Providers.Interfaces;
using IdentityModule.Application.Commands;
using IdentityModule.Application.Providers.Interfaces;
using IdentityModule.Application.Queries;
using IdentityModule.Domain.Entities;
using IdentityModule.Domain.Repositories.Interfaces;
using MongoDB.Driver;

namespace IdentityModule.Domain.Repositories
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

        public async Task<ServiceResponse> AddClaimPermission(AddClaimPermissionCommand command)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var claimPermission = AddClaimPermissionCommand.Initialize(command, authCtx);

            await _mongoDbContextProvider.InsertDocument(claimPermission);

            return Response.BuildServiceResponse().BuildSuccessResponse(claimPermission, authCtx?.RequestUri);
        }

        public async Task<ClaimPermission[]> GetClaimsForClaimNames(string[] claimNames)
        {
            var filter = Builders<ClaimPermission>.Filter.In(x => x.Name, claimNames);

            var results = await _mongoDbContextProvider.GetDocuments(filter: filter);

            return results is not null ? results.ToArray() : Array.Empty<ClaimPermission>();
        }

        public async Task<QueryRecordsResponse<ClaimPermission>> GetClaims(GetClaimsQuery query)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var filter = Builders<ClaimPermission>.Filter.Empty;

            var count = await _mongoDbContextProvider.CountDocuments(filter: filter);

            var clams = await _mongoDbContextProvider.GetDocuments(filter: filter);

            return Response.BuildQueryRecordsResponse<ClaimPermission>().BuildSuccessResponse(
               count: count,
               records: clams is not null ? clams.ToArray() : Array.Empty<ClaimPermission>(), requestUri: authCtx?.RequestUri);
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
