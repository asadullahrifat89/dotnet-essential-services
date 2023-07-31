using FluentValidation;
using IdentityCore.Contracts.Declarations.Commands;
using IdentityCore.Contracts.Declarations.Repositories;
using IdentityCore.Extensions;

namespace IdentityCore.Contracts.Implementations.Commands.Validators
{
    public class ValidateTokenCommandValidator : AbstractValidator<ValidateTokenCommand>
    {
        private readonly IAuthTokenRepository _authTokenRepository;

        public ValidateTokenCommandValidator(
            IAuthTokenRepository authTokenRepository)
        {
            _authTokenRepository = authTokenRepository;

            RuleFor(x => x.RefreshToken).NotNull().NotEmpty();
            RuleFor(x => x).MustAsync(BeAnExistingRefreshToken).WithMessage("Refresh token doesn't exists.").When(x => !x.RefreshToken.IsNullOrBlank());
        }

        private async Task<bool> BeAnExistingRefreshToken(ValidateTokenCommand command, CancellationToken arg2)
        {
            return await _authTokenRepository.BeAnExistingRefreshToken(refreshToken: command.RefreshToken);
        }
    }
}
