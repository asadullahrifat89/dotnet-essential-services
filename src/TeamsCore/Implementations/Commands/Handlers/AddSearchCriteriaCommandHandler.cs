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
    public class AddSearchCriteriaCommandHandler : IRequestHandler<AddSearchCriteriaCommand, ServiceResponse>
    {

        #region Fields

        private readonly ISearchCriteriaRepository _searchCriteriaRepository;
        private readonly AddSearchCriteriaCommandValidator _validator;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;
        private readonly ILogger<AddSearchCriteriaCommandHandler> _logger;

        #endregion

        #region Ctor

        public AddSearchCriteriaCommandHandler(
                       ISearchCriteriaRepository searchCriteriaRepository,
                                  AddSearchCriteriaCommandValidator validator,
                                             IAuthenticationContextProvider authenticationContextProvider,
                                                        ILogger<AddSearchCriteriaCommandHandler> logger)
        {
            _searchCriteriaRepository = searchCriteriaRepository;
            _validator = validator;
            _authenticationContextProvider = authenticationContextProvider;
            _logger = logger;
        }

        #endregion

        #region Methods
        
        public async Task<ServiceResponse> Handle(AddSearchCriteriaCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _searchCriteriaRepository.AddSearchCriteria(request);
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
