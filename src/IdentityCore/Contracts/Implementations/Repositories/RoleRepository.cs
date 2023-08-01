using IdentityCore.Contracts.Declarations.Commands;
using IdentityCore.Contracts.Declarations.Repositories;
using IdentityCore.Contracts.Declarations.Services;
using IdentityCore.Contracts.Implementations.Services;
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

        #endregion

        #region Ctor

        public RoleRepository(IMongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        public async Task<ServiceResponse> AddRole(AddRoleCommand command)
        {
            var role = Role.Initialize(command);

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

        public async Task<bool> BeAnExistingRole(string role)
        {
            var filter = Builders<Role>.Filter.Eq(x => x.Name, role);

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

            return results is not null? results.ToArray(): Array.Empty<UserRoleMap>();
        }

        #endregion
    }
}
