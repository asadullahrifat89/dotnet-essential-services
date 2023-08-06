using BaseCore.Extensions;
using BaseCore.Models.Responses;
using BaseCore.Services;
using EmailCore.Declarations.Commands;
using EmailCore.Declarations.Repositories;
using EmailCore.Implementations.Commands.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmailCore.Implementations.Commands.Handlers
{
    public class EnqueueEmailMessageCommandHandler : IRequestHandler<EnqueueEmailMessageCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<EnqueueEmailMessageCommandHandler> _logger;
        private readonly EnqueueEmailMessageCommandValidator _validator;
        private readonly IEmailMessageRepository _emailRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public EnqueueEmailMessageCommandHandler(
            ILogger<EnqueueEmailMessageCommandHandler> logger,
            EnqueueEmailMessageCommandValidator validator,
            IEmailMessageRepository emailRepository,
            IAuthenticationContextProvider authenticationContextProvider)
        {
            _logger = logger;
            _validator = validator;
            _emailRepository = emailRepository;
            _authenticationContextProvider = authenticationContextProvider;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> Handle(EnqueueEmailMessageCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _emailRepository.EnqueueEmailMessage(request);
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
