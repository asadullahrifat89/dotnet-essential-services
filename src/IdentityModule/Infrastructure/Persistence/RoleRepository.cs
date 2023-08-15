using BaseModule.Application.DTOs.Responses;
using BaseModule.Application.Providers.Interfaces;
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

        public async Task<ServiceResponse> AddRole(AddRoleCommand command)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var role = AddRoleCommand.Initialize(command, authCtx);

            var roleClaimMaps = new List<RoleClaimPermissionMap>();

            foreach (var claim in command.Claims.Distinct())
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

        public async Task<ServiceResponse> UpdateRole(UpdateRoleCommand command)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var existingRole = await _mongoDbContextProvider.FindById<Role>(command.RoleId);

            var newRoleClaimMaps = new List<RoleClaimPermissionMap>();

            if (command.Claims is not null && command.Claims.Any())
            {
                foreach (var claim in command.Claims.Distinct())
                {
                    var roleClaimMap = new RoleClaimPermissionMap()
                    {
                        RoleId = command.RoleId,
                        ClaimPermission = claim
                    };

                    newRoleClaimMaps.Add(roleClaimMap);
                }
            }

            var existingRoleClaimMaps = await _mongoDbContextProvider.GetDocuments(Builders<RoleClaimPermissionMap>.Filter.Eq(x => x.RoleId, command.RoleId));

            if (existingRoleClaimMaps != null && existingRoleClaimMaps.Any())
                await _mongoDbContextProvider.DeleteDocuments(Builders<RoleClaimPermissionMap>.Filter.In(x => x.Id, existingRoleClaimMaps.Select(x => x.Id).ToArray()));

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

        public async Task<QueryRecordsResponse<Role>> GetRoles(GetRolesQuery query)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            var filter = Builders<Role>.Filter.Empty;

            var count = await _mongoDbContextProvider.CountDocuments(filter: filter);

            var roles = await _mongoDbContextProvider.GetDocuments(filter: filter);

            return new QueryRecordsResponse<Role>().BuildSuccessResponse(
               count: count,
               records: roles is not null ? roles.ToArray() : Array.Empty<Role>(), authCtx?.RequestUri);
        }

        public async Task<QueryRecordsResponse<Role>> GetRolesByUserId(GetUserRolesQuery query)
        {
            var authCtx = _authenticationContextProvider.GetAuthenticationContext();

            // get user roles from user role map

            var userfilter = Builders<UserRoleMap>.Filter.Eq(x => x.UserId, query.UserId);

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
