using Base.Application.DTOs.Responses;
using Base.Application.Extensions;
using Identity.Application.Providers.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Teams.ContentMangement.Application.Commands.Validators;
using Teams.ContentMangement.Domain.Repositories.Interfaces;

namespace Teams.ContentMangement.Application.Commands.Handlers
{
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<AddProductCommandHandler> _logger;
        private readonly AddProductCommandValidator _validator;
        private readonly IProductRepository _productRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public AddProductCommandHandler(
            ILogger<AddProductCommandHandler> logger,
            AddProductCommandValidator validator,
            IProductRepository productRepository,
            IAuthenticationContextProvider authenticationContextProvider)
        {
            _logger = logger;
            _validator = validator;
            _productRepository = productRepository;
            _authenticationContextProvider = authenticationContextProvider;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> Handle(AddProductCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(command, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();
                var product = AddProductCommand.Map(command, authCtx);

                var result = await _productRepository.AddProduct(product, command.LinkedProductSearchCriteriaIds);

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
