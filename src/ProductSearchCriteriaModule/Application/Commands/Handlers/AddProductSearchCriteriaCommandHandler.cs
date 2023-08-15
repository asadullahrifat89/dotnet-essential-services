using BaseModule.Application.DTOs.Responses;
using BaseModule.Infrastructure.Extensions;
using IdentityModule.Application.Providers.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductSearchCriteriaModule.Application.Commands.Validators;
using ProductSearchCriteriaModule.Domain.Repositories.Interfaces;

namespace ProductSearchCriteriaModule.Application.Commands.Handlers
{
    public class AddProductSearchCriteriaCommandHandler : IRequestHandler<AddProductSearchCriteriaCommand, ServiceResponse>
    {

        #region Fields

        private readonly IProductSearchCriteriaRepository _ProductSearchCriteriaRepository;
        private readonly AddProductSearchCriteriaCommandValidator _validator;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;
        private readonly ILogger<AddProductSearchCriteriaCommandHandler> _logger;

        #endregion

        #region Ctor

        public AddProductSearchCriteriaCommandHandler(
            IProductSearchCriteriaRepository ProductSearchCriteriaRepository,
            AddProductSearchCriteriaCommandValidator validator,
            IAuthenticationContextProvider authenticationContextProvider,
            ILogger<AddProductSearchCriteriaCommandHandler> logger)
        {
            _ProductSearchCriteriaRepository = ProductSearchCriteriaRepository;
            _validator = validator;
            _authenticationContextProvider = authenticationContextProvider;
            _logger = logger;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> Handle(AddProductSearchCriteriaCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(command, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();
                var productSearchCriteria = AddProductSearchCriteriaCommand.Initialize(command, authCtx);

                return await _ProductSearchCriteriaRepository.AddProductSearchCriteria(productSearchCriteria);
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
