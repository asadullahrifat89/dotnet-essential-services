using FluentValidation;
using IdentityCore.Contracts.Declarations.Commands;
using IdentityCore.Contracts.Declarations.Repositories;
using IdentityCore.Extensions;

namespace IdentityCore.Contracts.Implementations.Commands.Validators
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
            RuleFor(x => x).Must(BeAnExistingClaimPermission).WithMessage("Claim doesn't exist.").When(x => !x.Name.IsNullOrBlank());
        }

        private async Task<bool> NotBeAnExistingRole(AddRoleCommand command, CancellationToken arg2)
        {
            return !await _roleRepository.BeAnExistingRole(role: command.Name);
        }
        private bool BeAnExistingClaimPermission(AddRoleCommand command)
        {
            return command.Claims.All(x => _claimPermissionRepository.BeAnExistingClaimPermission(claim: x));
        }
    }
}
