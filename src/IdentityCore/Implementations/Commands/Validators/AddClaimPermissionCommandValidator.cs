using BaseCore.Extensions;
using FluentValidation;
using IdentityCore.Declarations.Commands;
using IdentityCore.Declarations.Repositories;

namespace IdentityCore.Implementations.Commands.Validators
{
    public class AddClaimPermissionCommandValidator : AbstractValidator<AddClaimPermissionCommand>
    {
        private readonly IClaimPermissionRepository _claimPermissionRepository;

        public AddClaimPermissionCommandValidator(IClaimPermissionRepository ClaimPermissionRepository)
        {
            _claimPermissionRepository = ClaimPermissionRepository;

            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x).MustAsync(NotBeAnExistingClaimPermission).WithMessage("Name already exists.").When(x => !x.Name.IsNullOrBlank());

            // check if the request uri exists in the web api or not

            RuleFor(x => x.RequestUri).NotNull().NotEmpty();
            //RuleFor(x => x.RequestUri).Must(BeAnExistingRequestUri).WithMessage("RequestUri doesn't exist.").When(x => !x.RequestUri.IsNullOrBlank());
        }

        //private bool BeAnExistingRequestUri(string requestUri)
        //{
        //    return EndpointRoutes.GetEndpointRoutes().Contains(requestUri.ToLower());
        //}

        private async Task<bool> NotBeAnExistingClaimPermission(AddClaimPermissionCommand command, CancellationToken arg2)
        {
            return !await _claimPermissionRepository.BeAnExistingClaimPermission(claim: command.Name);
        }
    }
}
