using System.Security.Claims;

namespace IdentityCore.Contracts.Declarations.Repositories
{
    public interface IClaimPermissionRepository
    {
        bool BeAnExistingClaimPermission(string claim);
    }
}
