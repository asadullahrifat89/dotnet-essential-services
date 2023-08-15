using FluentValidation;
using IdentityModule.Domain.Repositories.Interfaces;

namespace IdentityModule.Application.Queries.Validators
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
