using BaseCore.Models.Responses;
using LingoCore.Declarations.Commands;
using MediatR;
using BaseCore.Extensions;
using BaseCore.Services;
using Microsoft.Extensions.Logging;
using LingoCore.Implementations.Commands.Validators;
using LingoCore.Declarations.Repositories;

namespace LingoCore.Implementations.Commands.Handlers
{
    public class AddLingoAppCommandHandler : IRequestHandler<AddLingoAppCommand, ServiceResponse>
    {

        #region Fields

        private readonly ILogger<AddLingoAppCommandHandler> _logger;

        private readonly AddLingoAppCommandValidator _validator;

        private readonly ILingoResourcesRepository _lingoResourcesRepository;

        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public AddLingoAppCommandHandler(
            ILogger<AddLingoAppCommandHandler> logger,
            AddLingoAppCommandValidator validator,
            ILingoResourcesRepository lingoResourcesRepository,
            IAuthenticationContextProvider authenticationContextProvider)
        {
            _logger = logger;
            _validator = validator;
            _lingoResourcesRepository = lingoResourcesRepository;
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

                return await _lingoResourcesRepository.AddLingoApp(request);
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
