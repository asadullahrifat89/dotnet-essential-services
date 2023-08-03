using FluentValidation;
using IdentityCore.Contracts.Declarations.Commands;
using IdentityCore.Contracts.Declarations.Repositories;
using IdentityCore.Contracts.Implementations.Repositories;
using IdentityCore.Extensions;


namespace IdentityCore.Contracts.Implementations.Commands.Validators
{
    public class UpdateUserRolesCommandValidator : AbstractValidator<UpdateUserRolesCommand>   
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;

        public UpdateUserRolesCommandValidator(IUserRepository userRepository, IRoleRepository roleRepository)
        {
            _userRepository = userRepository;
            _roleRepository = roleRepository;

            RuleFor(x => x.UserId).NotNull().NotEmpty();
            RuleFor(x => x.UserId).MustAsync(BeAnExistingUser).WithMessage("User doesn't exist.").When(x => !x.UserId.IsNullOrBlank());

            RuleFor(x => x).MustAsync(BeAnExistingRole).WithMessage("Role  doesn't exists.").When(x => x.RoleNames != null);

        }

        private async Task<bool> BeAnExistingUser(string userId, CancellationToken arg2)
        {
            return await _userRepository.BeAnExistingUser(userId);
        }

        private async Task<bool> BeAnExistingRole(UpdateUserRolesCommand command, CancellationToken arg2)
        {
            foreach (var role in command.RoleNames)
            {
                var exists = await _roleRepository.BeAnExistingRole(role);

                if (!exists)
                    return false;
            }

            return true;
        }

    }

}

