using BaseCore.Extensions;
using BaseCore.Models.Responses;
using IdentityCore.Declarations.Commands;
using IdentityCore.Declarations.Repositories;
using IdentityCore.Implementations.Commands.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityCore.Implementations.Commands.Handlers
{
    public class VerifyUserAccountActivationRequestCommandHandler : IRequestHandler<VerifyUserAccountActivationRequestCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<SendUserAccountActivationRequestCommandHandler> _logger;
        private readonly VerifyUserAccountActivationRequestCommandValidator _validator;
        private readonly IAccountActivationRequest _accountActivationRequest;

        #endregion

        public VerifyUserAccountActivationRequestCommandHandler(
            ILogger<SendUserAccountActivationRequestCommandHandler> logger,
            VerifyUserAccountActivationRequestCommandValidator validator,
            IAccountActivationRequest accountActivationRequest)
        {
            _logger = logger;
            _validator = validator;
            _accountActivationRequest = accountActivationRequest;
        }

        #region Methods

        public async Task<ServiceResponse> Handle(VerifyUserAccountActivationRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _accountActivationRequest.VerifyAccountActivationRequest(request);
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
