using FluentValidation;
using IdentityCore.Declarations.Queries;
using IdentityCore.Declarations.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
