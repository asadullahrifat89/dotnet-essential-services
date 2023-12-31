﻿using FluentValidation;
using Identity.Domain.Repositories.Interfaces;
using Base.Application.Extensions;

namespace Identity.Application.Commands.Validators
{
    public class AddRoleCommandValidator : AbstractValidator<AddRoleCommand>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IClaimPermissionRepository _claimPermissionRepository;

        public AddRoleCommandValidator(
            IRoleRepository roleRepository,
            IClaimPermissionRepository claimPermissionRepository)
        {
            _roleRepository = roleRepository;
            _claimPermissionRepository = claimPermissionRepository;

            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x).MustAsync(NotBeAnExistingRole).WithMessage("Role already exists.").When(x => !x.Name.IsNullOrBlank());

            RuleFor(x => x.Claims).NotNull();
            RuleFor(x => x).MustAsync(BeAnExistingClaimPermission).WithMessage("Claim doesn't exist.").When(x => x.Claims != null);
        }

        private async Task<bool> NotBeAnExistingRole(AddRoleCommand command, CancellationToken arg2)
        {
            return !await _roleRepository.BeAnExistingRole(role: command.Name);
        }

        private async Task<bool> BeAnExistingClaimPermission(AddRoleCommand command, CancellationToken arg2)
        {
            foreach (var claim in command.Claims)
            {
                var exists = await _claimPermissionRepository.BeAnExistingClaimPermission(claim);

                if (!exists)
                    return false;
            }

            return true;
        }
    }
}
