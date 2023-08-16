using BaseModule.Application.DTOs.Responses;
using BaseModule.Infrastructure.Extensions;
using IdentityModule.Application.Commands.Validators;
using IdentityModule.Application.Providers.Interfaces;
using IdentityModule.Domain.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityModule.Application.Commands.Handlers
{
    public class SendUserAccountActivationRequestCommandHandler : IRequestHandler<SendUserAccountActivationRequestCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<SendUserAccountActivationRequestCommandHandler> _logger;
        private readonly SendUserAccountActivationRequestCommandValidator _validator;
        private readonly IAccountActivationRequestRepository _accountActivationRequest;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public SendUserAccountActivationRequestCommandHandler(
            ILogger<SendUserAccountActivationRequestCommandHandler> logger,
            SendUserAccountActivationRequestCommandValidator validator,
            IAccountActivationRequestRepository accountActivationRequest,
            IAuthenticationContextProvider authenticationContextProvider)
        {
            _logger = logger;
            _validator = validator;
            _accountActivationRequest = accountActivationRequest;
            _authenticationContextProvider = authenticationContextProvider;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> Handle(SendUserAccountActivationRequestCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(command, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();

                var accountActivationRequest = SendUserAccountActivationRequestCommand.Initialize(command);

                return await _accountActivationRequest.CreateAccountActivationRequest(accountActivationRequest);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildServiceResponse().BuildErrorResponse(ex.Message);
            }
        }

        #endregion
    }
}
