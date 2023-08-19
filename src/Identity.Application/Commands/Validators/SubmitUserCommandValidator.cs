using FluentValidation;
using Base.Application.Extensions;
using Identity.Domain.Repositories.Interfaces;
using System.Text.RegularExpressions;

namespace Identity.Application.Commands.Validators
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
            RuleFor(x => x.Email).Must(IsValidEmail).WithMessage("Invalid email.").When(x => !x.Email.IsNullOrBlank());
            RuleFor(x => x.Email).Must(e => e.Contains('@')).WithMessage("Invalid email.").When(x => !x.Email.IsNullOrBlank());
            RuleFor(x => x.Email).MustAsync(NotBeAnExistingUserEmail).WithMessage("Email already exists.").When(x => !x.Email.IsNullOrBlank());

            //TODO: validate tenant id
        }

        private async Task<bool> NotBeAnExistingUserEmail(string email, CancellationToken token)
        {
            return !await _userRepository.BeAnExistingUserEmail(email);
        }

        public bool IsValidEmail(string email)
        {
            // Regular expression pattern for validating email addresses
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            // Use Regex.IsMatch to check if the email matches the pattern
            return Regex.IsMatch(email, pattern);
        }
    }
}
