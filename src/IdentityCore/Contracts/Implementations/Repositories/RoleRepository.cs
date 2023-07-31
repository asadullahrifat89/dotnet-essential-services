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

            return Response.Build().BuildSuccessResponse(role);
        }

        public async Task<bool> BeAnExistingRole(string role)
        {
            var filter = Builders<Role>.Filter.Eq(x => x.Name, role);

            return await _mongoDbService.Exists(filter);
        }

        public async Task<QueryRecordsResponse<Role>> GetRolesByNames(string[] names)
        {
            var filter = Builders<Role>.Filter.In(x => x.Name, names);

            var count = await _mongoDbService.CountDocuments(filter);

            var results = await _mongoDbService.GetDocuments(filter: filter);

            return new QueryRecordsResponse<Role>().BuildSuccessResponse(
              count: results is not null ? count : 0,
              records: results is not null ? results.ToArray() : Array.Empty<Role>());
        }

        #endregion
    }
}
