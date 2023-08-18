using Base.Application.Extensions;
using Base.Infrastructure.Providers.Interfaces;
using Identity.Application.Providers.Interfaces;
using Identity.Domain.Entities;
using Identity.Domain.Repositories.Interfaces;
using MongoDB.Driver;
using System.Data;

namespace Identity.Infrastructure.Persistence
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

        public async Task<Role> AddRole(Role role, string[] claims)
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

            return role;
        }

        public async Task<Role> UpdateRole(string roleId, string[] claims)
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

            return existingRole;
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

        public async Task<(long Count, Role[] Roles)> GetRoles(string searchTerm, int pageIndex, int pageSize)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var filter = Builders<Role>.Filter.Empty;

            if (!searchTerm.IsNullOrBlank())
            {
                filter &= Builders<Role>.Filter.Where(x => x.Name.ToLower().Contains(searchTerm.ToLower()));
            }

            var count = await _mongoDbContextProvider.CountDocuments(filter: filter);

            var roles = await _mongoDbContextProvider.GetDocuments(filter: filter, skip: pageIndex * pageSize, limit: pageSize);

            return (count, roles is not null ? roles.ToArray() : Array.Empty<Role>());
        }

        public async Task<(long Count, Role[] Roles)> GetRolesByUserId(string userId)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            // get user roles from user role map

            var userfilter = Builders<UserRoleMap>.Filter.Eq(x => x.UserId, userId);

            var userRoleMaps = await _mongoDbContextProvider.GetDocuments(filter: userfilter);

            var roleIds = userRoleMaps.Select(x => x.RoleId).ToArray();

            // get roles by id from roles collection

            var roleFilter = Builders<Role>.Filter.In(x => x.Id, roleIds);

            var roles = await _mongoDbContextProvider.GetDocuments(filter: roleFilter);

            return (roles.Count(), roles is not null ? roles.ToArray() : Array.Empty<Role>());
        }

        #endregion
    }
}
