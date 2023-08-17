using FluentValidation;
using Identity.Domain.Repositories.Interfaces;

namespace Identity.Application.Queries.Validators
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
