using MediatR;
using Microsoft.Extensions.Logging;
using Identity.Application.Commands.Validators;
using Identity.Application.Providers.Interfaces;
using Identity.Domain.Repositories.Interfaces;
using Base.Application.DTOs.Responses;
using Base.Application.Extensions;
using Identity.Application.DTOs;

namespace Identity.Application.Commands.Handlers
{
    public class CreateUserCommandHandler : IRequestHandler<CreateUserCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<CreateUserCommandHandler> _logger;
        private readonly CreateUserCommandValidator _validator;
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public CreateUserCommandHandler(
            ILogger<CreateUserCommandHandler> logger,
            CreateUserCommandValidator validator,
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

        public async Task<ServiceResponse> Handle(CreateUserCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(command, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();

                var user = CreateUserCommand.Map(command, authCtx);

                var result = await _userRepository.CreateUser(user, command.Roles);

                return Response.BuildServiceResponse().BuildSuccessResponse(UserResponse.Map(result), authCtx.RequestUri);
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
