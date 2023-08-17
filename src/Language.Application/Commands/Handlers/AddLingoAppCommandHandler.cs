using MediatR;
using Microsoft.Extensions.Logging;
using LanguageModule.Domain.Repositories.Interfaces;
using LanguageModule.Application.Commands.Validators;
using Base.Application.DTOs.Responses;
using Identity.Application.Providers.Interfaces;
using Base.Application.Extensions;

namespace LanguageModule.Application.Commands.Handlers
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
