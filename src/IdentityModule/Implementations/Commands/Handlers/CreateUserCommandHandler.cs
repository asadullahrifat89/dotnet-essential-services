using MediatR;
using Microsoft.Extensions.Logging;
using IdentityModule.Declarations.Repositories;
using IdentityModule.Declarations.Commands;
using BaseModule.Models.Responses;
using BaseModule.Services;
using IdentityModule.Implementations.Commands.Validators;
using BaseModule.Extensions;

namespace IdentityModule.Implementations.Commands.Handlers
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

        public async Task<ServiceResponse> Handle(CreateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _userRepository.CreateUser(request);
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
