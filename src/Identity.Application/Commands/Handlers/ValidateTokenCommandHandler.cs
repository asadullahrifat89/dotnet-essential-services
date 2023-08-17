using MediatR;
using Microsoft.Extensions.Logging;
using Identity.Application.Commands.Validators;
using Base.Application.DTOs.Responses;
using Identity.Domain.Repositories.Interfaces;
using Base.Application.Extensions;
using Identity.Application.Providers.Interfaces;

namespace Identity.Application.Commands.Handlers
{
    public class ValidateTokenCommandHandler : IRequestHandler<ValidateTokenCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<ValidateTokenCommandHandler> _logger;
        private readonly ValidateTokenCommandValidator _validator;
        private readonly IAuthTokenRepository _repository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public ValidateTokenCommandHandler(
            ILogger<ValidateTokenCommandHandler> logger,
            ValidateTokenCommandValidator validator,
            IAuthTokenRepository repository,
            IAuthenticationContextProvider authenticationContextProvider)
        {
            _logger = logger;
            _validator = validator;
            _repository = repository;
            _authenticationContextProvider = authenticationContextProvider;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> Handle(ValidateTokenCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(command, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();
                var result = await _repository.ValidateToken(command.RefreshToken);

                return Response.BuildServiceResponse().BuildSuccessResponse(result, authCtx?.RequestUri);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildServiceResponse().BuildErrorResponse(ex.Message, _authenticationContextProvider.GetAuthenticationContext().RequestUri);
            }
        }

        #endregion
    }
}
