using Base.Application.DTOs.Responses;
using Base.Application.Extensions;
using Base.Shared.Constants;
using Email.Domain.Repositories.Interfaces;
using Identity.Application.Commands.Validators;
using Identity.Application.DTOs;
using Identity.Application.Providers.Interfaces;
using Identity.Domain.Entities;
using Identity.Domain.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Teams.UserManagement.Application.Commands.Handlers
{
    public class OnboardUserCommandHandler : IRequestHandler<OnboardUserCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<OnboardUserCommandHandler> _logger;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;
        private readonly IMediator _mediator;
        private readonly IEmailTemplateRepository _emailTemplateRepository;
        private readonly IConfiguration _configuration;

        #endregion

        #region Ctor

        public OnboardUserCommandHandler(
            ILogger<OnboardUserCommandHandler> logger,
            IAuthenticationContextProvider authenticationContextProvider,
            IEmailTemplateRepository emailTemplateRepository,
            IMediator mediator,
            IConfiguration configuration)
        {
            _logger = logger;
            _authenticationContextProvider = authenticationContextProvider;
            _emailTemplateRepository = emailTemplateRepository;
            _mediator = mediator;
            _configuration = configuration;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> Handle(OnboardUserCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var authCtx = _authenticationContextProvider.GetAuthenticationContext();

                // submit user
                var submitUserCommand = OnboardUserCommand.InitializeSubmitUserCommand(command);
                var userSubmitted = await _mediator.Send(submitUserCommand);

                if (userSubmitted.IsSuccess)
                {
                    // send user activation request
                    var sendUserAccountActivationRequestCommand = OnboardUserCommand.InitializeSendUserAccountActivationRequestCommand(command);
                    var userActivationSubmitted = await _mediator.Send(sendUserAccountActivationRequestCommand);

                    if (userActivationSubmitted.IsSuccess)
                    {
                        var accountActivationRequest = userActivationSubmitted.Result as AccountActivationRequest;
                        var activationLink = _configuration["Routes:BaseUrl"] + "/" + _configuration["Routes:Onboarding"];

                        // get email template for user onborading purpose
                        var emailTemplate = await _emailTemplateRepository.GetEmailTemplateByPurpose("account-activation");

                        var enqueueEmailCommand = OnboardUserCommand.InitializeEnqueueEmailMessageCommand(
                               command: command,
                               activationKey: accountActivationRequest.ActivationKey,
                               activationLink: activationLink,
                               emailTemplate: emailTemplate);

                        // send email

                        var emailEnqueued = await _mediator.Send(enqueueEmailCommand);

                        return emailEnqueued;
                    }
                    else
                    {
                        return userActivationSubmitted;
                    }
                }
                else
                {
                    return userSubmitted;
                }
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
