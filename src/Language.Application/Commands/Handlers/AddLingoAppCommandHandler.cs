using MediatR;
using Microsoft.Extensions.Logging;
using Base.Application.DTOs.Responses;
using Identity.Application.Providers.Interfaces;
using Base.Application.Extensions;
using Language.Application.Commands.Validators;
using Language.Domain.Repositories.Interfaces;

namespace Language.Application.Commands.Handlers
{
    public class AddLingoAppCommandHandler : IRequestHandler<AddLingoAppCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<AddLingoAppCommandHandler> _logger;
        private readonly AddLingoAppCommandValidator _validator;
        private readonly ILanguageAppRepository _lingoAppRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public AddLingoAppCommandHandler(
            ILogger<AddLingoAppCommandHandler> logger,
            AddLingoAppCommandValidator validator,
            ILanguageAppRepository lingoAppRepository,
            IAuthenticationContextProvider authenticationContextProvider)
        {
            _logger = logger;
            _validator = validator;
            _lingoAppRepository = lingoAppRepository;
            _authenticationContextProvider = authenticationContextProvider;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> Handle(AddLingoAppCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(command, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();

                var lingoApp = AddLingoAppCommand.Initialize(command, authCtx);

                var result = await _lingoAppRepository.AddLingoApp(lingoApp);

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
