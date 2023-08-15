using BaseModule.Extensions;
using FluentValidation;
using IdentityModule.Declarations.Queries;
using IdentityModule.Declarations.Repositories;

namespace IdentityModule.Implementations.Queries.Validators
{
    public class GetUserRolesQueryValidator : AbstractValidator<GetUserRolesQuery>
    {
        private readonly IUserRepository _userRepository;

        public GetUserRolesQueryValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(x => x.UserId).NotNull().NotEmpty();
            RuleFor(x => x.UserId).MustAsync(BeAnExistingUser).WithMessage("User doesn't exist.").When(x => !x.UserId.IsNullOrBlank());
        }

        private async Task<bool> BeAnExistingUser(string userId, CancellationToken arg2)
        {
            return await _userRepository.BeAnExistingUser(userId);
        }
    }
}
