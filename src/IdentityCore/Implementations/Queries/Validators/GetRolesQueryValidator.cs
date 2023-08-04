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
    public class GetRolesQueryValidator : AbstractValidator<GetRolesQuery>
    {
        private readonly IRoleRepository _roleRepository;

        public GetRolesQueryValidator(IRoleRepository roleRepository)
        {
            _roleRepository = roleRepository;
        }
    }
}
