using BaseCore.Extensions;
using FluentValidation;
using IdentityCore.Declarations.Commands;
using IdentityCore.Declarations.Repositories;

namespace IdentityCore.Implementations.Commands.Validators
{
    public class VerifyUserAccountActivationRequestCommandValidator : AbstractValidator<VerifyUserAccountActivationRequestCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountActivationRequest _accountActivationRequest;


        public VerifyUserAccountActivationRequestCommandValidator(IUserRepository userRepository, IAccountActivationRequest accountActivationRequest)
        {
            _userRepository = userRepository;
            _accountActivationRequest = accountActivationRequest;

            RuleFor(x => x.Email).NotNull().NotEmpty();
            RuleFor(x => x.ActivationKey).NotNull().NotEmpty();
            RuleFor(x => x.Password).NotNull().NotEmpty();
            RuleFor(x => x).MustAsync(BeAnExistingUser).WithMessage("User doesn't exist.").When(x => !x.Email.IsNullOrBlank());
        }

        private Task<bool> BeAnExistingUser(VerifyUserAccountActivationRequestCommand command, CancellationToken token)
        {
            return _userRepository.BeAnExistingUserEmail(command.Email);
        }
    }
}
