using IdentityCore.Contracts.Declarations.Repositories;
using IdentityCore.Contracts.Declarations.Services;

namespace IdentityCore.Contracts.Implementations.Repositories
{
    public class ClaimPermissionRepository : IClaimPermissionRepository
    {
        #region Fields

        private readonly IMongoDbService _mongoDbService;

        #endregion

        #region Ctor

        public ClaimPermissionRepository(IMongoDbService mongoDbService)
        {
            _mongoDbService = mongoDbService;
        }

        #region Methods

        public bool BeAnExistingClaimPermission(string claim)
        {
            // TODO: check in constants if this claim exists or not            

            return Constants.Claims.Contains(claim);
        }

        #endregion

        #endregion
    }
}
