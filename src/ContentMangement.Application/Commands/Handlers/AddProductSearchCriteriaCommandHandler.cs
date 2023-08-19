using Base.Application.DTOs.Responses;
using Base.Application.Extensions;
using Identity.Application.Providers.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Teams.ContentMangement.Application.Commands.Validators;
using Teams.ContentMangement.Domain.Repositories.Interfaces;

namespace Teams.ContentMangement.Application.Commands.Handlers
{
    public class AddProductSearchCriteriaCommandHandler : IRequestHandler<AddProductSearchCriteriaCommand, ServiceResponse>
    {
        #region Fields

        private readonly IProductSearchCriteriaRepository _searchCriteriaRepository;
        private readonly AddProductSearchCriteriaCommandValidator _validator;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;
        private readonly ILogger<AddProductSearchCriteriaCommandHandler> _logger;

        #endregion

        #region Ctor

        public AddProductSearchCriteriaCommandHandler(
            IProductSearchCriteriaRepository searchCriteriaRepository,
            AddProductSearchCriteriaCommandValidator validator,
            IAuthenticationContextProvider authenticationContextProvider,
            ILogger<AddProductSearchCriteriaCommandHandler> logger)
        {
            _searchCriteriaRepository = searchCriteriaRepository;
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
                var searchCriteria = AddProductSearchCriteriaCommand.Map(command, authCtx);

                var result = await _searchCriteriaRepository.AddProductSearchCriteria(searchCriteria);

                return Response.BuildServiceResponse().BuildSuccessResponse(result, authCtx?.RequestUri);
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
