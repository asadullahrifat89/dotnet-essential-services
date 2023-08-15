using BaseModule.Infrastructure.Extensions;
using FluentValidation;
using IdentityModule.Declarations.Commands;
using IdentityModule.Declarations.Repositories;

namespace IdentityModule.Implementations.Commands.Validators
{
    public class VerifyUserAccountActivationRequestCommandValidator : AbstractValidator<VerifyUserAccountActivationRequestCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountActivationRequestRepository _accountActivationRequestRepository;

        public VerifyUserAccountActivationRequestCommandValidator(IUserRepository userRepository, IAccountActivationRequestRepository accountActivationRequestRepository)
        {
            _userRepository = userRepository;
            _accountActivationRequestRepository = accountActivationRequestRepository;

            RuleFor(x => x.ActivationKey).NotNull().NotEmpty();
            RuleFor(x => x.ActivationKey).MustAsync(BeAnExistingActivationKey).WithMessage("Activation key is not valid.").When(x => !x.ActivationKey.IsNullOrBlank());

            RuleFor(x => x.Password).NotNull().NotEmpty();

            RuleFor(x => x.Email).NotNull().NotEmpty();
            RuleFor(x => x).MustAsync(BeAnExistingUser).WithMessage("User doesn't exist.").When(x => !x.Email.IsNullOrBlank());
        }

        private async Task<bool> BeAnExistingActivationKey(string activationKey, CancellationToken token)
        {
            return await _accountActivationRequestRepository.BeAnExistingActivationKey(activationKey);
        }

        private Task<bool> BeAnExistingUser(VerifyUserAccountActivationRequestCommand command, CancellationToken token)
        {
            return _userRepository.BeAnExistingUserEmail(command.Email);
        }
    }
}
