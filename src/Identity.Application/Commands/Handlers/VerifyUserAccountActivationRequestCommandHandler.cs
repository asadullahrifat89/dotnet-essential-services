using Base.Application.DTOs.Responses;
using Base.Application.Extensions;
using Identity.Application.Commands.Validators;
using Identity.Application.DTOs;
using Identity.Application.Providers.Interfaces;
using Identity.Domain.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Identity.Application.Commands.Handlers
{
    public class VerifyUserAccountActivationRequestCommandHandler : IRequestHandler<VerifyUserAccountActivationRequestCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<SendUserAccountActivationRequestCommandHandler> _logger;
        private readonly VerifyUserAccountActivationRequestCommandValidator _validator;
        private readonly IAccountActivationRequestRepository _accountActivationRequest;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public VerifyUserAccountActivationRequestCommandHandler(
           ILogger<SendUserAccountActivationRequestCommandHandler> logger,
           VerifyUserAccountActivationRequestCommandValidator validator,
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

        public async Task<ServiceResponse> Handle(VerifyUserAccountActivationRequestCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(command, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();
                var result = await _accountActivationRequest.VerifyAccountActivationRequest(
                    email: command.Email,
                    activationKey: command.ActivationKey,
                    password: command.Password);

                return Response.BuildServiceResponse().BuildSuccessResponse(UserResponse.Map(result), authCtx.RequestUri);
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
