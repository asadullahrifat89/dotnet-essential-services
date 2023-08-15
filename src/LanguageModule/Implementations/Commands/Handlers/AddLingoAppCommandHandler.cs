using MediatR;
using Microsoft.Extensions.Logging;
using LanguageModule.Declarations.Commands;
using LanguageModule.Implementations.Commands.Validators;
using LanguageModule.Declarations.Repositories;
using BaseModule.Infrastructure.Extensions;
using BaseModule.Infrastructure.Services.Interfaces;
using BaseModule.Application.DTOs.Responses;

namespace LanguageModule.Implementations.Commands.Handlers
{
    public class AddLingoAppCommandHandler : IRequestHandler<AddLingoAppCommand, ServiceResponse>
    {

        #region Fields

        private readonly ILogger<AddLingoAppCommandHandler> _logger;

        private readonly AddLingoAppCommandValidator _validator;

        private readonly ILingoAppRepository _lingoAppRepository;

        private readonly IAuthenticationContextProviderService _authenticationContextProvider;

        #endregion

        #region Ctor

        public AddLingoAppCommandHandler(
            ILogger<AddLingoAppCommandHandler> logger,
            AddLingoAppCommandValidator validator,
            ILingoAppRepository lingoAppRepository,
            IAuthenticationContextProviderService authenticationContextProvider)
        {
            _logger = logger;
            _validator = validator;
            _lingoAppRepository = lingoAppRepository;
            _authenticationContextProvider = authenticationContextProvider;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> Handle(AddLingoAppCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _lingoAppRepository.AddLingoApp(request);
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
