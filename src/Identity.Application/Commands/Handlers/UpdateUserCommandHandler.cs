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
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<UpdateUserCommandHandler> _logger;
        private readonly UpdateUserCommandValidator _validator;
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public UpdateUserCommandHandler(
            ILogger<UpdateUserCommandHandler> logger,
            UpdateUserCommandValidator validator,
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

        public async Task<ServiceResponse> Handle(UpdateUserCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(command, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();

                var user = UpdateUserCommand.Initialize(command, authCtx);

                var result = await _userRepository.UpdateUser(user);

                return Response.BuildServiceResponse().BuildSuccessResponse(UserResponse.Initialize(result), authCtx?.RequestUri);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildServiceResponse().BuildErrorResponse(ex.Message);
            }
        }

        #endregion
    }
}
