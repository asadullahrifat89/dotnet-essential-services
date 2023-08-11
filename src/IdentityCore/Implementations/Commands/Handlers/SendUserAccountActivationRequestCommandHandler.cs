using BaseCore.Extensions;
using BaseCore.Models.Responses;
using IdentityCore.Declarations.Commands;
using IdentityCore.Declarations.Repositories;
using IdentityCore.Implementations.Commands.Validators;
using IdentityCore.Implementations.Repositories;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Implementations.Commands.Handlers
{
    public class SendUserAccountActivationRequestCommandHandler : IRequestHandler<SendUserAccountActivationRequestCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<SendUserAccountActivationRequestCommandHandler> _logger;
        private readonly SendUserAccountActivationRequestCommandValidator _validator;
        private readonly IAccountActivationRequestRepository _accountActivationRequest;

        #endregion

        #region Ctor

        public SendUserAccountActivationRequestCommandHandler(
            ILogger<SendUserAccountActivationRequestCommandHandler> logger,
            SendUserAccountActivationRequestCommandValidator validator,
            IAccountActivationRequestRepository accountActivationRequest)
        {
            _logger = logger;
            _validator = validator;
            _accountActivationRequest = accountActivationRequest;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> Handle(SendUserAccountActivationRequestCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _accountActivationRequest.CreateAccountActivationRequest(request);
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
