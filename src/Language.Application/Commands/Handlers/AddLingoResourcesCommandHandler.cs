using Base.Application.DTOs.Responses;
using Base.Application.Extensions;
using Identity.Application.Providers.Interfaces;
using Language.Application.Commands;
using Language.Application.Commands.Validators;
using Language.Domain.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Language.Application.Commands.Handlers
{
    public class AddLingoResourcesCommandHandler : IRequestHandler<AddLingoResourcesCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<AddLingoResourcesCommandHandler> _logger;

        private readonly AddLingoResourcesCommandValidator _validator;

        private readonly ILanguageResourcesRepository _lingoResourcesRepository;

        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public AddLingoResourcesCommandHandler(
            ILogger<AddLingoResourcesCommandHandler> logger,
            AddLingoResourcesCommandValidator validator,
            ILanguageResourcesRepository lingoResourcesRepository,
            IAuthenticationContextProvider authenticationContextProvider)
        {
            _logger = logger;
            _validator = validator;
            _lingoResourcesRepository = lingoResourcesRepository;
            _authenticationContextProvider = authenticationContextProvider;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> Handle(AddLingoResourcesCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(command, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();

                var lingoResources = AddLingoResourcesCommand.Initialize(command, authCtx);

                var result = await _lingoResourcesRepository.AddLanguageResources(lingoResources);

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
