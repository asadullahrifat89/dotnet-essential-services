using MediatR;
using Microsoft.Extensions.Logging;
using BaseModule.Infrastructure.Extensions;
using BaseModule.Application.DTOs.Responses;
using IdentityModule.Application.Commands.Validators;
using IdentityModule.Domain.Repositories.Interfaces;
using IdentityModule.Application.Providers.Interfaces;
using MongoDB.Driver;

namespace IdentityModule.Application.Commands.Handlers
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

        public CreateUserCommandHandler(ILogger<CreateUserCommandHandler> logger, CreateUserCommandValidator validator, IUserRepository userRepository, IAuthenticationContextProvider authenticationContextProvider)
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

                var user = CreateUserCommand.Initialize(command, authCtx);

                return await _userRepository.CreateUser(user, command.Roles);
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
