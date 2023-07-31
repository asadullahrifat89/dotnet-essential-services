using FluentValidation;
using IdentityCore.Contracts.Declarations.Commands;
using IdentityCore.Contracts.Declarations.Repositories;
using IdentityCore.Contracts.Implementations.Repositories;
using IdentityCore.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Contracts.Implementations.Commands.Validators
{
    public class CreateUserCommandValidator : AbstractValidator<CreateUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public CreateUserCommandValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(x => x.Email).NotNull().NotEmpty();
            RuleFor(x => x.Email).Must(e => e.Contains('@')).WithMessage("Invalid email.").When(x => !x.Email.IsNullOrBlank());
            RuleFor(x => x.Email).MustAsync(NotBeAnExistingUserEmail).WithMessage("Email already exists.").When(x => !x.Email.IsNullOrBlank());

            RuleFor(x => x.Password).NotNull().NotEmpty();
        }

        private async Task<bool> NotBeAnExistingUserEmail(string email, CancellationToken token)
        {
            return !await _userRepository.BeAnExistingUserEmail(email);
        }
    }
}
