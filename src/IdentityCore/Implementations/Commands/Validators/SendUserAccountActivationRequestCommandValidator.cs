using BaseCore.Extensions;
using FluentValidation;
using IdentityCore.Declarations.Commands;
using IdentityCore.Declarations.Repositories;


namespace IdentityCore.Implementations.Commands.Validators
{
    public class SendUserAccountActivationRequestCommandValidator : AbstractValidator<SendUserAccountActivationRequestCommand>
    {
        private readonly IUserRepository _userRepository;
        private readonly IAccountActivationRequest _accountActivationReques;

        public SendUserAccountActivationRequestCommandValidator(
            IUserRepository userRepository,
            IAccountActivationRequest accountActivationReques)
        {
            _userRepository = userRepository;
            _accountActivationReques = accountActivationReques;

            RuleFor(x => x.Email).NotNull().NotEmpty();
            RuleFor(x => x).MustAsync(BeAnExistingUser).WithMessage("User doesn't exist.").When(x => !x.Email.IsNullOrBlank());

        }

        private Task<bool> BeAnExistingUser(SendUserAccountActivationRequestCommand command, CancellationToken token)
        {
            return _userRepository.BeAnExistingUserEmail(command.Email);
            
        }
    }
   
}
