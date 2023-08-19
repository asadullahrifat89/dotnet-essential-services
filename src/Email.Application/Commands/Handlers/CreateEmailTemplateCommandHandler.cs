using Base.Application.DTOs.Responses;
using Base.Application.Extensions;
using Email.Application.Commands.Validators;
using Email.Domain.Repositories.Interfaces;
using Identity.Application.Providers.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Email.Application.Commands.Handlers
{
    public class CreateEmailTemplateCommandHandler : IRequestHandler<CreateEmailTemplateCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<CreateEmailTemplateCommandHandler> _logger;
        private readonly CreateEmailTemplateCommandValidator _validator;
        private readonly IEmailTemplateRepository _emailRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public CreateEmailTemplateCommandHandler(
            ILogger<CreateEmailTemplateCommandHandler> logger,
            CreateEmailTemplateCommandValidator validator,
            IEmailTemplateRepository emailRepository,
            IAuthenticationContextProvider authenticationContextProvider)
        {
            _logger = logger;
            _validator = validator;
            _emailRepository = emailRepository;
            _authenticationContextProvider = authenticationContextProvider;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> Handle(CreateEmailTemplateCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(command, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();
                
                var emailTemplate = CreateEmailTemplateCommand.Map(command, authCtx);

                var result = await _emailRepository.CreateEmailTemplate(emailTemplate);

                return Response.BuildServiceResponse().BuildSuccessResponse(result, authCtx.RequestUri);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildServiceResponse().BuildErrorResponse(ex.Message, _authenticationContextProvider.GetAuthenticationContext().RequestUri);
            }
        }

        #endregion
    }
}
