using Base.Application.DTOs.Responses;
using Base.Application.Extensions;
using Base.Shared.Constants;
using Email.Application.Queries;
using Email.Domain.Repositories.Interfaces;
using Identity.Application.Commands.Validators;
using Identity.Application.DTOs;
using Identity.Application.Providers.Interfaces;
using Identity.Domain.Entities;
using Identity.Domain.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Teams.UserManagement.Application.Commands.Validators;

namespace Teams.UserManagement.Application.Commands.Handlers
{
    public class OnboardUserCommandHandler : IRequestHandler<OnboardUserCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<OnboardUserCommandHandler> _logger;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        private readonly OnboardUserCommandValidator _validator;

        #endregion

        #region Ctor

        public OnboardUserCommandHandler(
            ILogger<OnboardUserCommandHandler> logger,
            IAuthenticationContextProvider authenticationContextProvider,
            IMediator mediator,
            IConfiguration configuration,
            OnboardUserCommandValidator validator)
        {
            _logger = logger;
            _authenticationContextProvider = authenticationContextProvider;
            _mediator = mediator;
            _configuration = configuration;
            _validator = validator;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> Handle(OnboardUserCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var authCtx = _authenticationContextProvider.GetAuthenticationContext();

                var validationResult = await _validator.ValidateAsync(command, cancellationToken);
                validationResult.EnsureValidResult();

                // submit user
                var submitUserCommand = OnboardUserCommand.MapSubmitUserCommand(command);
                var userSubmitted = await _mediator.Send(submitUserCommand, cancellationToken);

                if (userSubmitted.IsSuccess)
                {
                    // send user activation request
                    var sendUserAccountActivationRequestCommand = OnboardUserCommand.MapSendUserAccountActivationRequestCommand(command);
                    var userActivationSubmitted = await _mediator.Send(sendUserAccountActivationRequestCommand, cancellationToken);

                    if (userActivationSubmitted.IsSuccess)
                    {
                        var accountActivationRequest = userActivationSubmitted.Result as AccountActivationRequest;
                        var activationLink = _configuration["Routes:AppUrl"] + "/" + _configuration["Routes:Onboarding"] + $"?email={command.Email}";

                        // get email template for user onborading purpose
                        var emailTemplateQuery = new GetEmailTemplateByPurposeQuery() { Purpose = "account-activation" };
                        var emailTemplateAcquired = await _mediator.Send(emailTemplateQuery, cancellationToken);

                        var enqueueEmailCommand = OnboardUserCommand.MapEnqueueEmailMessageCommand(
                              command: command,
                              activationKey: accountActivationRequest.ActivationKey,
                              activationLink: activationLink,
                              emailTemplate: emailTemplateAcquired.IsSuccess ? emailTemplateAcquired.Result : default);

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
                return Response.BuildServiceResponse().BuildErrorResponse(ex.Message, _authenticationContextProvider.GetAuthenticationContext().RequestUri);
            }
        }

        #endregion
    }
}
