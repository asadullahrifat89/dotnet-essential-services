using BaseCore.Extensions;
using BaseCore.Models.Responses;
using BaseCore.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using TeamsCore.Declarations.Commands;
using TeamsCore.Declarations.Repositories;
using TeamsCore.Implementations.Commands.Validators;

namespace TeamsCore.Implementations.Commands.Handlers
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

        public async Task<ServiceResponse> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _productRepository.UpdateProduct(request);
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
