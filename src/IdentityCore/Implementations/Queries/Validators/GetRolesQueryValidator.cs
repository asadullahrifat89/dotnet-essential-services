using FluentValidation;
using IdentityCore.Declarations.Queries;
using IdentityCore.Declarations.Repositories;

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
