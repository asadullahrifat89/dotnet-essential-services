using MediatR;
using Microsoft.Extensions.Logging;
using Identity.Application.Commands.Validators;
using Identity.Application.Providers.Interfaces;
using Base.Application.DTOs.Responses;
using Identity.Domain.Repositories.Interfaces;
using Base.Application.Extensions;
using Identity.Application.DTOs;

namespace Identity.Application.Commands.Handlers
{
    public class SubmitUserCommandHandler : IRequestHandler<SubmitUserCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<SubmitUserCommandHandler> _logger;
        private readonly SubmitUserCommandValidator _validator;
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public SubmitUserCommandHandler(
            ILogger<SubmitUserCommandHandler> logger,
            SubmitUserCommandValidator validator,
            IUserRepository userRepository,
            IAuthenticationContextProvider authenticationContextProvider)
        {
            _logger = logger;
            _validator = validator;
            _userRepository = userRepository;
            _authenticationContextProvider = authenticationContextProvider;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> Handle(SubmitUserCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(command, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();
                var user = SubmitUserCommand.Initialize(command, authCtx);

                var result = await _userRepository.SubmitUser(user);

                return Response.BuildServiceResponse().BuildSuccessResponse(UserResponse.Initialize(result), authCtx?.RequestUri);
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
