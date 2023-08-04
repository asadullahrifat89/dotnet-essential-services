using FluentValidation;
using IdentityCore.Declarations.Queries;
using IdentityCore.Declarations.Repositories;

namespace IdentityCore.Implementations.Queries.Validators
{
    public class GetClaimsQueryValidator : AbstractValidator<GetClaimsQuery>
    {
        private readonly IClaimPermissionRepository _claimPermissionRepository;

        public GetClaimsQueryValidator(IClaimPermissionRepository claimPermissionRepository)
        {
            _claimPermissionRepository = claimPermissionRepository;
        }
    }
}
