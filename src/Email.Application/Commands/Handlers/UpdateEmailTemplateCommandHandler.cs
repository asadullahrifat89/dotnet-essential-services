using MediatR;
using Microsoft.Extensions.Logging;
using Email.Application.Commands.Validators;
using Email.Domain.Repositories.Interfaces;
using Base.Application.DTOs.Responses;
using Identity.Application.Providers.Interfaces;
using Base.Application.Extensions;

namespace Email.Application.Commands.Handlers
{
    public class UpdateEmailTemplateCommandHandler : IRequestHandler<UpdateEmailTemplateCommand, ServiceResponse>
    {
        private readonly ILogger<UpdateEmailTemplateCommand> _logger;
        private readonly UpdateEmailTemplateCommandValidator _validator;
        private readonly IEmailTemplateRepository _emailRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        public UpdateEmailTemplateCommandHandler(
            ILogger<UpdateEmailTemplateCommand> logger,
            UpdateEmailTemplateCommandValidator validator,
            IEmailTemplateRepository emailRepository,
            IAuthenticationContextProvider authenticationContextProvider)
        {
            _logger = logger;
            _validator = validator;
            _emailRepository = emailRepository;
            _authenticationContextProvider = authenticationContextProvider;
        }

        public async Task<ServiceResponse> Handle(UpdateEmailTemplateCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(command, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();
                
                var emailTemplate = UpdateEmailTemplateCommand.Map(command, authCtx);

                var updatedTemplate = await _emailRepository.UpdateEmailTemplate(emailTemplate);
                
                return Response.BuildServiceResponse().BuildSuccessResponse(updatedTemplate, authCtx?.RequestUri);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildServiceResponse().BuildErrorResponse(ex.Message);
            }
        }
    }
}
