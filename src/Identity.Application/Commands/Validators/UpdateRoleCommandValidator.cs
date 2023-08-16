using FluentValidation;
using Identity.Domain.Repositories.Interfaces;
using Base.Application.Extensions;

namespace Identity.Application.Commands.Validators
{
    public class UpdateRoleCommandValidator : AbstractValidator<UpdateRoleCommand>
    {
        private readonly IRoleRepository _roleRepository;
        private readonly IClaimPermissionRepository _claimPermissionRepository;

        public UpdateRoleCommandValidator(
            IRoleRepository roleRepository,
            IClaimPermissionRepository claimPermissionRepository)
        {
            _roleRepository = roleRepository;
            _claimPermissionRepository = claimPermissionRepository;

            RuleFor(x => x.RoleId).NotNull().NotEmpty();
            RuleFor(x => x).MustAsync(BeAnExistingRole).WithMessage("Role doesn't exists.").When(x => !x.RoleId.IsNullOrBlank());

            RuleFor(x => x).MustAsync(BeAnExistingClaimPermission).WithMessage("Claim doesn't exist.").When(x => x.Claims != null);
        }

        private async Task<bool> BeAnExistingRole(UpdateRoleCommand command, CancellationToken arg2)
        {
            return await _roleRepository.BeAnExistingRoleById(id: command.RoleId);
        }

        private async Task<bool> BeAnExistingClaimPermission(UpdateRoleCommand command, CancellationToken arg2)
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
