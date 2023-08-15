using BaseModule.Infrastructure.Extensions;
using FluentValidation;
using IdentityModule.Domain.Repositories.Interfaces;

namespace IdentityModule.Application.Commands.Validators
{
    public class SubmitUserCommandValidator : AbstractValidator<SubmitUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        public SubmitUserCommandValidator(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;

            RuleFor(x => x.Email).NotNull().NotEmpty();
            RuleFor(x => x.Email).Must(e => e.Contains('@')).WithMessage("Invalid email.").When(x => !x.Email.IsNullOrBlank());
            RuleFor(x => x.Email).MustAsync(NotBeAnExistingUserEmail).WithMessage("Email already exists.").When(x => !x.Email.IsNullOrBlank());

            //TODO: validate tenant id
        }

        private async Task<bool> NotBeAnExistingUserEmail(string email, CancellationToken token)
        {
            return !await _userRepository.BeAnExistingUserEmail(email);
        }
    }
}
