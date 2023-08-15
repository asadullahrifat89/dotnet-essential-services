using BaseModule.Infrastructure.Extensions;
using FluentValidation;
using IdentityModule.Declarations.Queries;
using IdentityModule.Declarations.Repositories;

namespace IdentityModule.Implementations.Queries.Validators
{
    public class GetUserQueryValidator : AbstractValidator<GetUserQuery>
    {
        private readonly IUserRepository _userRepository;

        public GetUserQueryValidator(IUserRepository userRepository)
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
