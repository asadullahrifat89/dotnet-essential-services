using BaseModule.Application.DTOs.Responses;
using BaseModule.Infrastructure.Extensions;
using BaseModule.Infrastructure.Services.Interfaces;
using EmailModule.Domain.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace EmailModule.Application.Commands
{
    public class CreateEmailTemplateCommandHandler : IRequestHandler<CreateEmailTemplateCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<CreateEmailTemplateCommandHandler> _logger;
        private readonly CreateEmailTemplateCommandValidator _validator;
        private readonly IEmailTemplateRepository _emailRepository;
        private readonly IAuthenticationContextProviderService _authenticationContextProvider;

        #endregion

        #region Ctor

        public CreateEmailTemplateCommandHandler(
            ILogger<CreateEmailTemplateCommandHandler> logger,
            CreateEmailTemplateCommandValidator validator,
            IEmailTemplateRepository emailRepository,
            IAuthenticationContextProviderService authenticationContextProvider)
        {
            _logger = logger;
            _validator = validator;
            _emailRepository = emailRepository;
            _authenticationContextProvider = authenticationContextProvider;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> Handle(CreateEmailTemplateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _emailRepository.CreateEmailTemplate(request);
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
