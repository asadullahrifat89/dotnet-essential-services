﻿using Base.Application.DTOs.Responses;
using Base.Application.Extensions;
using Identity.Application.Providers.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Teams.ContentMangement.Application.Commands.Validators;
using Teams.ContentMangement.Domain.Repositories.Interfaces;

namespace Teams.ContentMangement.Application.Commands.Handlers
{
    public class UpdateProductSearchCriteriaCommandHandler : IRequestHandler<UpdateProductSearchCriteriaCommand, ServiceResponse>
    {
        #region Fields

        private readonly IProductSearchCriteriaRepository _ProductSearchCriteriaRepository;
        private readonly UpdateProductSearchCriteriaCommandValidator _validator;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;
        private readonly ILogger<UpdateProductSearchCriteriaCommandHandler> _logger;

        #endregion

        #region Ctor

        public UpdateProductSearchCriteriaCommandHandler(
            IProductSearchCriteriaRepository ProductSearchCriteriaRepository,
            UpdateProductSearchCriteriaCommandValidator validator,
            IAuthenticationContextProvider authenticationContextProvider,
            ILogger<UpdateProductSearchCriteriaCommandHandler> logger)
        {
            _ProductSearchCriteriaRepository = ProductSearchCriteriaRepository;
            _validator = validator;
            _authenticationContextProvider = authenticationContextProvider;
            _logger = logger;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> Handle(UpdateProductSearchCriteriaCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(command, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();
                var ProductSearchCriteria = UpdateProductSearchCriteriaCommand.Map(command, authCtx);

                var result = await _ProductSearchCriteriaRepository.UpdateProductSearchCriteria(ProductSearchCriteria);

                return Response.BuildServiceResponse().BuildSuccessResponse(result, authCtx.RequestUri);
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
