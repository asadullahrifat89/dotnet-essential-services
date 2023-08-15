using BaseModule.Application.DTOs.Responses;
using BaseModule.Infrastructure.Extensions;
using EmailModule.Application.Commands.Validators;
using EmailModule.Domain.Interfaces;
using IdentityModule.Infrastructure.Services.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmailModule.Application.Commands.Handlers
{
    public class EnqueueEmailMessageCommandHandler : IRequestHandler<EnqueueEmailMessageCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<EnqueueEmailMessageCommandHandler> _logger;
        private readonly EnqueueEmailMessageCommandValidator _validator;
        private readonly IEmailMessageRepository _emailRepository;
        private readonly IAuthenticationContextProviderService _authenticationContextProvider;

        #endregion

        #region Ctor

        public EnqueueEmailMessageCommandHandler(
            ILogger<EnqueueEmailMessageCommandHandler> logger,
            EnqueueEmailMessageCommandValidator validator,
            IEmailMessageRepository emailRepository,
            IAuthenticationContextProviderService authenticationContextProvider)
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
