using BaseModule.Infrastructure.Extensions;
using FluentValidation;
using IdentityModule.Domain.Repositories.Interfaces;

namespace IdentityModule.Application.Queries.Validators
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
