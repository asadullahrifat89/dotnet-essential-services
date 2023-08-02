using FluentValidation;
using IdentityCore.Contracts.Declarations.Commands;
using IdentityCore.Contracts.Declarations.Repositories;
using IdentityCore.Extensions;

namespace IdentityCore.Contracts.Implementations.Commands.Validators
{
    public class AddClaimPermissionCommandValidator : AbstractValidator<AddClaimPermissionCommand>
    {
        private readonly IClaimPermissionRepository _claimPermissionRepository;

        public AddClaimPermissionCommandValidator(IClaimPermissionRepository ClaimPermissionRepository)
        {
            _claimPermissionRepository = ClaimPermissionRepository;

            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x).MustAsync(NotBeAnExistingClaimPermission).WithMessage("Name already exists.").When(x => !x.Name.IsNullOrBlank());

            //TODO: check if the request uri exists in the web api or not
        }

        private async Task<bool> NotBeAnExistingClaimPermission(AddClaimPermissionCommand command, CancellationToken arg2)
        {
            return !await _claimPermissionRepository.BeAnExistingClaimPermission(claim: command.Name);
        }
    }
}
