using Base.Application.DTOs.Responses;
using Base.Application.Extensions;
using Identity.Application.Providers.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Teams.ContentMangement.Application.Commands.Validators;
using Teams.ContentMangement.Domain.Repositories.Interfaces;

namespace Teams.ContentMangement.Application.Commands.Handlers
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, ServiceResponse>
    {

        #region Fields

        private readonly IProductRepository _productRepository;
        private readonly UpdateProductCommandValidator _validator;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;
        private readonly ILogger<UpdateProductCommandHandler> _logger;

        #endregion

        #region Ctor

        public UpdateProductCommandHandler(
            IProductRepository productRepository,
            UpdateProductCommandValidator validator,
            IAuthenticationContextProvider authenticationContextProvider,
            ILogger<UpdateProductCommandHandler> logger)
        {
            _productRepository = productRepository;
            _validator = validator;
            _authenticationContextProvider = authenticationContextProvider;
            _logger = logger;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> Handle(UpdateProductCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(command, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();
                var product = UpdateProductCommand.Map(command, authCtx);

                var result = await _productRepository.UpdateProduct(product, command.LinkedProductSearchCriteriaIds);
                return Response.BuildServiceResponse().BuildSuccessResponse(result, authCtx.RequestUri);
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
