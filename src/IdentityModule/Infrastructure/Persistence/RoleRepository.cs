using BaseModule.Application.DTOs.Responses;
using BaseModule.Application.Providers.Interfaces;
using BaseModule.Infrastructure.Extensions;
using IdentityModule.Application.Commands;
using IdentityModule.Application.Providers.Interfaces;
using IdentityModule.Application.Queries;
using IdentityModule.Domain.Entities;
using IdentityModule.Domain.Repositories.Interfaces;
using MongoDB.Driver;
using System.Data;

namespace IdentityModule.Infrastructure.Persistence
{
    public class RoleRepository : IRoleRepository
    {
        #region Fields

        private readonly IMongoDbContextProvider _mongoDbContextProvider;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public RoleRepository(IMongoDbContextProvider mongoDbService, IAuthenticationContextProvider authenticationContext)
        {
            _mongoDbContextProvider = mongoDbService;
            _authenticationContextProvider = authenticationContext;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> AddRole(Role role, string[] claims)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var roleClaimMaps = new List<RoleClaimPermissionMap>();

            foreach (var claim in claims.Distinct())
            {
                var roleClaimMap = new RoleClaimPermissionMap()
                {
                    RoleId = role.Id,
                    ClaimPermission = claim
                };

                roleClaimMaps.Add(roleClaimMap);
            }

            await _mongoDbContextProvider.InsertDocument(role);
            await _mongoDbContextProvider.InsertDocuments(roleClaimMaps);

            return Response.BuildServiceResponse().BuildSuccessResponse(role, authCtx?.RequestUri);
        }

        public async Task<ServiceResponse> UpdateRole(string roleId, string[] claims)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var existingRole = await _mongoDbContextProvider.FindById<Role>(roleId);

            var newRoleClaimMaps = new List<RoleClaimPermissionMap>();

            if (claims is not null && claims.Any())
            {
                foreach (var claim in claims.Distinct())
                {
                    var roleClaimMap = new RoleClaimPermissionMap()
                    {
                        RoleId = roleId,
                        ClaimPermission = claim
                    };

                    newRoleClaimMaps.Add(roleClaimMap);
                }
            }

            // remove existing role claim maps
            await _mongoDbContextProvider.DeleteDocuments(Builders<RoleClaimPermissionMap>.Filter.Eq(x => x.RoleId, roleId));

            if (newRoleClaimMaps.Any())
                await _mongoDbContextProvider.InsertDocuments(newRoleClaimMaps);

            return Response.BuildServiceResponse().BuildSuccessResponse(existingRole, authCtx?.RequestUri);
        }

        public async Task<bool> BeAnExistingRole(string role)
        {
            var filter = Builders<Role>.Filter.Eq(x => x.Name, role);

            return await _mongoDbContextProvider.Exists(filter);
        }

        public async Task<bool> BeAnExistingRoleById(string id)
        {
            var filter = Builders<Role>.Filter.Eq(x => x.Id, id);

            return await _mongoDbContextProvider.Exists(filter);
        }

        public async Task<Role[]> GetRolesByNames(string[] names)
        {
            var filter = Builders<Role>.Filter.In(x => x.Name, names);

            var results = await _mongoDbContextProvider.GetDocuments(filter: filter);

            return results is not null ? results.ToArray() : Array.Empty<Role>();
        }

        public async Task<UserRoleMap[]> GetUserRoles(string userId)
        {
            var filter = Builders<UserRoleMap>.Filter.Eq(x => x.UserId, userId);

            var results = await _mongoDbContextProvider.GetDocuments(filter: filter);

            return results is not null ? results.ToArray() : Array.Empty<UserRoleMap>();
        }

        public async Task<QueryRecordsResponse<Role>> GetRoles(string searchTerm, int pageIndex, int pageSize)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var filter = Builders<Role>.Filter.Empty;

            if (!searchTerm.IsNullOrBlank())
            {
                filter &= Builders<Role>.Filter.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            var count = await _mongoDbContextProvider.CountDocuments(filter: filter);

            var roles = await _mongoDbContextProvider.GetDocuments(filter: filter, skip: pageIndex * pageSize, limit: pageSize);

            return new QueryRecordsResponse<Role>().BuildSuccessResponse(
               count: count,
               records: roles is not null ? roles.ToArray() : Array.Empty<Role>(), authCtx?.RequestUri);
        }

        public async Task<QueryRecordsResponse<Role>> GetRolesByUserId(string userId)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            // get user roles from user role map

            var userfilter = Builders<UserRoleMap>.Filter.Eq(x => x.UserId, userId);

            var userRoleMaps = await _mongoDbContextProvider.GetDocuments(filter: userfilter);

            var roleIds = userRoleMaps.Select(x => x.RoleId).ToArray();

            // get roles by id from roles collection

            var roleFilter = Builders<Role>.Filter.In(x => x.Id, roleIds);

            var roles = await _mongoDbContextProvider.GetDocuments(filter: roleFilter);

            return Response.BuildQueryRecordsResponse<Role>().BuildSuccessResponse(
                   count: roles.Count(),
                   records: roles is not null ? roles.ToArray() : Array.Empty<Role>(), authCtx?.RequestUri);
        }

        #endregion
    }
}
