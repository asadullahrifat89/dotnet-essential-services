using BaseModule.Infrastructure.Extensions;
using FluentValidation;
using IdentityModule.Declarations.Commands;
using IdentityModule.Declarations.Repositories;

namespace IdentityModule.Implementations.Commands.Validators
{
    public class UpdateUserPasswordCommandValidator : AbstractValidator<UpdateUserPasswordCommand>
    {
        private readonly IUserRepository _userRepository;

        public UpdateUserPasswordCommandValidator(IUserRepository userRepository)
        {
            _userRepository = userRepository;

            RuleFor(x => x.UserId).NotNull().NotEmpty();
            RuleFor(x => x.UserId).MustAsync(BeAnExistingUser).WithMessage("User doesn't exist.").When(x => !x.UserId.IsNullOrBlank());

            RuleFor(x => x.OldPassword).NotNull().NotEmpty();
            RuleFor(x => x).MustAsync(BeValidUserPassword).WithMessage("Old password doesn't exist.").When(x => !x.UserId.IsNullOrBlank() && !x.OldPassword.IsNullOrBlank());

            RuleFor(x => x.NewPassword).NotNull().NotEmpty();
            RuleFor(x => x).Must(x => x.NewPassword != x.OldPassword).WithMessage("New password can not be the same as the old password.").When(x => !x.UserId.IsNullOrBlank() && !x.NewPassword.IsNullOrBlank() && !x.OldPassword.IsNullOrBlank());
            RuleFor(x => x.NewPassword).Must(BeStrongPassword).WithMessage("New password not strong enough.").When(x => !x.NewPassword.IsNullOrBlank());
        }

        private async Task<bool> BeAnExistingUser(string userId, CancellationToken arg2)
        {
            return await _userRepository.BeAnExistingUser(userId);
        }

        private async Task<bool> BeValidUserPassword(UpdateUserPasswordCommand command, CancellationToken arg2)
        {
            return await _userRepository.BeValidUserPassword(userId: command.UserId, password: command.OldPassword);
        }

        public static bool BeStrongPassword(string passwd)
        {
            if (passwd.IsNullOrBlank() || passwd.Length < 8 || passwd.Length > 14 || !passwd.Any(char.IsUpper) || !passwd.Any(char.IsLower) || passwd.Contains(" "))
                return false;

            string specialCh = @"%!@#$%^&*()?/>.<,:;'\|}]{[_~`+=-" + "\"";
            char[] specialChArray = specialCh.ToCharArray();
            char[] passArray = passwd.ToCharArray();

            if (!passArray.Any(x => specialChArray.Contains(x)))
                return false;

            return true;
        }
    }
}
