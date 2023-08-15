using MediatR;
using Microsoft.Extensions.Logging;
using BaseModule.Infrastructure.Extensions;
using BaseModule.Application.DTOs.Responses;
using IdentityModule.Infrastructure.Services.Interfaces;
using LanguageModule.Domain.Repositories.Interfaces;
using LanguageModule.Application.Commands;
using LanguageModule.Application.Commands.Validators;

namespace LanguageModule.Application.Commands.Handlers
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
