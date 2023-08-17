using MediatR;
using Microsoft.Extensions.Logging;
using Identity.Application.Commands.Validators;
using Base.Application.DTOs.Responses;
using Identity.Domain.Repositories.Interfaces;
using Base.Application.Extensions;
using Identity.Application.Providers.Interfaces;

namespace Identity.Application.Commands.Handlers
{
    public class UpdateUserRolesCommandHandler : IRequestHandler<UpdateUserRolesCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<UpdateUserRolesCommandHandler> _logger;
        private readonly UpdateUserRolesCommandValidator _validator;
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public UpdateUserRolesCommandHandler(
            ILogger<UpdateUserRolesCommandHandler> logger,
            UpdateUserRolesCommandValidator validator,
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

        public async Task<ServiceResponse> Handle(UpdateUserRolesCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(command, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();
                var result = await _userRepository.UpdateUserRoles(userId: command.UserId, roleNames: command.RoleNames);

                return Response.BuildServiceResponse().BuildSuccessResponse(result, authCtx?.RequestUri);
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
