using BaseCore.Extensions;
using BaseCore.Models.Responses;
using BaseCore.Services;
using LingoCore.Declarations.Commands;
using LingoCore.Declarations.Repositories;
using LingoCore.Implementations.Commands.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LingoCore.Implementations.Commands.Handlers
{
    public class AddLingoResourcesCommandHandler : IRequestHandler<AddLingoResourcesCommand, ServiceResponse>
    {
            #region Fields

            private readonly ILogger<AddLingoResourcesCommandHandler> _logger;

            private readonly AddLingoResourcesCommandValidator _validator;

            private readonly ILingoResourcesRepository _lingoResourcesRepository;

            private readonly IAuthenticationContextProvider _authenticationContextProvider;

            #endregion

            #region Ctor

            public AddLingoResourcesCommandHandler(
                ILogger<AddLingoResourcesCommandHandler> logger,
                AddLingoResourcesCommandValidator validator,
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

            public async Task<ServiceResponse> Handle(AddLingoResourcesCommand request, CancellationToken cancellationToken)
            {
                try
                {
                    var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                    validationResult.EnsureValidResult();

                    return await _lingoResourcesRepository.AddLingoResources(request);
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
