using FluentValidation;
using IdentityCore.Contracts.Declarations.Commands;
using IdentityCore.Contracts.Declarations.Repositories;
using IdentityCore.Extensions;

namespace IdentityCore.Contracts.Implementations.Commands.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        public CreateUserCommandValidator(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;

            RuleFor(x => x.FirstName).NotNull().NotEmpty();
            RuleFor(x => x.LastName).NotNull().NotEmpty();

            RuleFor(x => x.Email).NotNull().NotEmpty();
            RuleFor(x => x.Email).Must(e => e.Contains('@')).WithMessage("Invalid email.").When(x => !x.Email.IsNullOrBlank());
            RuleFor(x => x.Email).MustAsync(NotBeAnExistingUserEmail).WithMessage("Email already exists.").When(x => !x.Email.IsNullOrBlank());

            RuleFor(x => x.Password).NotNull().NotEmpty();

            RuleFor(x => x.ProfileImageUrl).NotNull().NotEmpty();

            RuleFor(x => x.PhoneNumber).NotNull().NotEmpty();
            RuleFor(x => x.PhoneNumber).MustAsync(NotBeAnExistingPhoneNumber).WithMessage("Phone number is already in use.").When(x => !x.Email.IsNullOrBlank());

            RuleFor(x => x.Roles).NotNull();
            RuleFor(x => x).MustAsync(BeAnExistingRole).WithMessage("Role doesn't exist.").When(x => x.Roles != null);
        }

        private async Task<bool> NotBeAnExistingUserEmail(string email, CancellationToken token)
        {
            return !await _userRepository.BeAnExistingUserEmail(email);
        }

        private async Task<bool> NotBeAnExistingPhoneNumber(string phoneNumber, CancellationToken token)
        {
            return !await _userRepository.BeAnExistingPhoneNumber(phoneNumber);
        }

        private async Task<bool> BeAnExistingRole(CreateUserCommand command, CancellationToken token)
        {
            foreach (var role in command.Roles)
            {
                var exists = await _roleRepository.BeAnExistingRole(role);

                if (!exists)
                    return false;
            }

            return true;
        }
    }
}
