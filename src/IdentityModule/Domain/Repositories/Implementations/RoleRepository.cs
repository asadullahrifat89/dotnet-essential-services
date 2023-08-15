using BaseModule.Application.DTOs.Responses;
using BaseModule.Application.Providers.Interfaces;
using IdentityModule.Application.Commands;
using IdentityModule.Application.Queries;
using IdentityModule.Domain.Entities;
using IdentityModule.Domain.Repositories.Interfaces;
using IdentityModule.Infrastructure.Services.Interfaces;
using MongoDB.Driver;
using System.Data;

namespace IdentityModule.Domain.Repositories.Implementations
{
    public class RoleRepository : IRoleRepository
    {
        #region Fields

        private readonly IMongoDbContextProvider _mongoDbService;
        private readonly IAuthenticationContextProviderService _authenticationContext;

        #endregion

        #region Ctor

        public RoleRepository(IMongoDbContextProvider mongoDbService, IAuthenticationContextProviderService authenticationContext)
        {
            _mongoDbService = mongoDbService;
            _authenticationContext = authenticationContext;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> AddRole(AddRoleCommand command)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var role = Role.Initialize(command, authCtx);

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

            await _mongoDbService.InsertDocument(role);
            await _mongoDbService.InsertDocuments(roleClaimMaps);

            return Response.BuildServiceResponse().BuildSuccessResponse(role, authCtx?.RequestUri);
        }

        public async Task<ServiceResponse> UpdateRole(UpdateRoleCommand command)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var existingRole = await _mongoDbService.FindById<Role>(command.RoleId);

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

            var existingRoleClaimMaps = await _mongoDbService.GetDocuments(Builders<RoleClaimPermissionMap>.Filter.Eq(x => x.RoleId, command.RoleId));

            if (existingRoleClaimMaps != null && existingRoleClaimMaps.Any())
                await _mongoDbService.DeleteDocuments(Builders<RoleClaimPermissionMap>.Filter.In(x => x.Id, existingRoleClaimMaps.Select(x => x.Id).ToArray()));

            if (newRoleClaimMaps.Any())
                await _mongoDbService.InsertDocuments(newRoleClaimMaps);

            return Response.BuildServiceResponse().BuildSuccessResponse(existingRole, authCtx?.RequestUri);
        }

        public async Task<bool> BeAnExistingRole(string role)
        {
            var filter = Builders<Role>.Filter.Eq(x => x.Name, role);

            return await _mongoDbService.Exists(filter);
        }

        public async Task<bool> BeAnExistingRoleById(string id)
        {
            var filter = Builders<Role>.Filter.Eq(x => x.Id, id);

            return await _mongoDbService.Exists(filter);
        }

        public async Task<Role[]> GetRolesByNames(string[] names)
        {
            var filter = Builders<Role>.Filter.In(x => x.Name, names);

            var results = await _mongoDbService.GetDocuments(filter: filter);

            return results is not null ? results.ToArray() : Array.Empty<Role>();
        }

        public async Task<UserRoleMap[]> GetUserRoles(string userId)
        {
            var filter = Builders<UserRoleMap>.Filter.Eq(x => x.UserId, userId);

            var results = await _mongoDbService.GetDocuments(filter: filter);

            return results is not null ? results.ToArray() : Array.Empty<UserRoleMap>();
        }

        public async Task<QueryRecordsResponse<Role>> GetRoles(GetRolesQuery query)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            var filter = Builders<Role>.Filter.Empty;

            var count = await _mongoDbService.CountDocuments(filter: filter);

            var roles = await _mongoDbService.GetDocuments(filter: filter);

            return new QueryRecordsResponse<Role>().BuildSuccessResponse(
               count: count,
               records: roles is not null ? roles.ToArray() : Array.Empty<Role>(), authCtx?.RequestUri);
        }

        public async Task<QueryRecordsResponse<Role>> GetRolesByUserId(GetUserRolesQuery query)
        {
            var authCtx = _authenticationContext.GetAuthenticationContext();

            // get user roles from user role map

            var userfilter = Builders<UserRoleMap>.Filter.Eq(x => x.UserId, query.UserId);

            var userRoleMaps = await _mongoDbService.GetDocuments(filter: userfilter);

            var roleIds = userRoleMaps.Select(x => x.RoleId).ToArray();

            // get roles by id from roles collection

            var roleFilter = Builders<Role>.Filter.In(x => x.Id, roleIds);

            var roles = await _mongoDbService.GetDocuments(filter: roleFilter);

            return Response.BuildQueryRecordsResponse<Role>().BuildSuccessResponse(
                   count: roles.Count(),
                   records: roles is not null ? roles.ToArray() : Array.Empty<Role>(), authCtx?.RequestUri);
        }

        #endregion
    }
}
