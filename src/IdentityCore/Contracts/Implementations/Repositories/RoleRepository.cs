using IdentityCore.Contracts.Declarations.Commands;
using IdentityCore.Contracts.Declarations.Queries;
using IdentityCore.Contracts.Declarations.Repositories;
using IdentityCore.Contracts.Declarations.Services;
using IdentityCore.Models.Entities;
using IdentityCore.Models.Responses;
using MongoDB.Driver;
using System.Data;

namespace IdentityCore.Contracts.Implementations.Repositories
{
    public class RoleRepository : IRoleRepository
    {
        #region Fields

        private readonly IMongoDbService _mongoDbService;
        private readonly IAuthenticationContextProvider _authenticationContext;

        #endregion

        #region Ctor

        public RoleRepository(IMongoDbService mongoDbService, IAuthenticationContextProvider authenticationContext)
        {
            _mongoDbService = mongoDbService;
            _authenticationContext = authenticationContext;
        }

        public async Task<ServiceResponse> AddRole(AddRoleCommand command)
        {
            var role = Role.Initialize(command, _authenticationContext.GetAuthenticationContext());

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

            return Response.BuildServiceResponse().BuildSuccessResponse(role);
        }

        public async Task<ServiceResponse> UpdateRole(UpdateRoleCommand command)
        {
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

            return Response.BuildServiceResponse().BuildSuccessResponse(existingRole);
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
            var filter = Builders<Role>.Filter.Empty;

            var count = await _mongoDbService.CountDocuments(filter: filter);

            var roles = await _mongoDbService.GetDocuments(filter: filter);

            return new QueryRecordsResponse<Role>().BuildSuccessResponse(
               count: count,
               records: roles is not null ? roles.ToArray() : Array.Empty<Role>()
               );
        }


        public async Task<QueryRecordsResponse<Role>> GetRoleByUserId(GetRoleQuery query)
        {
            // get user roles from user role map

            var userfilter = Builders<UserRoleMap>.Filter.Eq(x => x.UserId, query.UserId);

            var userRoleMaps = await _mongoDbService.GetDocuments(filter: userfilter);

            var roleIds = userRoleMaps.Select(x => x.RoleId).ToArray();

            // get roles by id from roles collection

            var roleFilter = Builders<Role>.Filter.In(x => x.Id, roleIds);

            var roles = await _mongoDbService.GetDocuments(filter: roleFilter);

            return Response.BuildQueryRecordsResponse<Role>().BuildSuccessResponse(
                   count: roles.Count(),
                   records: roles is not null ? roles.ToArray() : Array.Empty<Role>());

          
        }

        

        #endregion
    }
}
