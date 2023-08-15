﻿using BaseModule.Application.DTOs.Responses;
using BaseModule.Infrastructure.Extensions;
using IdentityModule.Application.Commands.Validators;
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