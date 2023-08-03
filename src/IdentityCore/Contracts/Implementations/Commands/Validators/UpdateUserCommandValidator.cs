﻿using FluentValidation;
using IdentityCore.Contracts.Declarations.Commands;
using IdentityCore.Contracts.Declarations.Repositories;
using IdentityCore.Extensions;

namespace IdentityCore.Contracts.Implementations.Commands.Validators
{
    public class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserCommandValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(x => x.UserId).NotNull().NotEmpty();
            RuleFor(x => x.UserId).MustAsync(BeAnExistingUser).WithMessage("User doesn't exist.").When(x => !x.UserId.IsNullOrBlank());

            RuleFor(x => x.FirstName).NotNull().NotEmpty();
            RuleFor(x => x.LastName).NotNull().NotEmpty();
        }

        private async Task<bool> BeAnExistingUser(string userId, CancellationToken arg2)
        {
            return await _userRepository.BeAnExistingUser(userId);
        }
    }
}
