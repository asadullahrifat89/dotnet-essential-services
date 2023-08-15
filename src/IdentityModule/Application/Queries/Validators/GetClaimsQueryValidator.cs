using FluentValidation;
using IdentityModule.Domain.Repositories.Interfaces;

namespace IdentityModule.Application.Queries.Validators
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
