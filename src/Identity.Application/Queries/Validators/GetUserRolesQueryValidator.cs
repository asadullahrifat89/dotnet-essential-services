﻿using Base.Application.Extensions;
using FluentValidation;
using Identity.Domain.Repositories.Interfaces;

namespace Identity.Application.Queries.Validators
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
