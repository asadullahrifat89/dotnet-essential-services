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
    public class AddProductCommandHandler : IRequestHandler<AddProductCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<AddProjectCommandHandler> _logger;
        private readonly AddProductCommandValidator _validator;
        private readonly IProductRepository _productRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public AddProductCommandHandler(ILogger<AddProjectCommandHandler> logger, AddProductCommandValidator validator, IProductRepository productRepository, IAuthenticationContextProvider authenticationContextProvider)
        {
            _logger = logger;
            _validator = validator;
            _productRepository = productRepository;
            _authenticationContextProvider = authenticationContextProvider;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> Handle(AddProductCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _productRepository.AddProduct(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildServiceResponse().BuildErrorResponse(ex.Message, _authenticationContextProvider.GetAuthenticationContext()?.RequestUri);
            }
        }

        #endregion
    }
}
