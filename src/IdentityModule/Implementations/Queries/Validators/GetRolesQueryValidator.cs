using FluentValidation;
using IdentityModule.Declarations.Queries;
using IdentityModule.Declarations.Repositories;

namespace IdentityModule.Implementations.Queries.Validators
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
