using BaseModule.Application.DTOs.Responses;
using BaseModule.Infrastructure.Extensions;
using IdentityModule.Application.Commands.Validators;
using IdentityModule.Domain.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityModule.Application.Commands.Handlers
{
    public class VerifyUserAccountActivationRequestCommandHandler : IRequestHandler<VerifyUserAccountActivationRequestCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<SendUserAccountActivationRequestCommandHandler> _logger;
        private readonly VerifyUserAccountActivationRequestCommandValidator _validator;
        private readonly IAccountActivationRequestRepository _accountActivationRequest;

        #endregion

        #region Ctor

        public VerifyUserAccountActivationRequestCommandHandler(
           ILogger<SendUserAccountActivationRequestCommandHandler> logger,
           VerifyUserAccountActivationRequestCommandValidator validator,
           IAccountActivationRequestRepository accountActivationRequest)
        {
            _logger = logger;
            _validator = validator;
            _accountActivationRequest = accountActivationRequest;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> Handle(VerifyUserAccountActivationRequestCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(command, cancellationToken);
                validationResult.EnsureValidResult();

                return await _accountActivationRequest.VerifyAccountActivationRequest(
                    email: command.Email,
                    activationKey: command.ActivationKey,
                    password: command.Password);
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
