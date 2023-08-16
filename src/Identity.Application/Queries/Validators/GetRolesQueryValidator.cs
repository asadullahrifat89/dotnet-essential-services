using FluentValidation;
using Identity.Domain.Repositories.Interfaces;

namespace Identity.Application.Queries.Validators
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
