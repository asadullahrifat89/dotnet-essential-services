using FluentValidation;
using IdentityModule.Declarations.Queries;
using IdentityModule.Declarations.Repositories;

namespace IdentityModule.Implementations.Queries.Validators
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
