using MediatR;
using Microsoft.Extensions.Logging;
using IdentityModule.Declarations.Repositories;
using BaseModule.Models.Responses;
using BaseModule.Services;
using IdentityModule.Declarations.Commands;
using IdentityModule.Implementations.Commands.Validators;
using BaseModule.Extensions;

namespace IdentityModule.Implementations.Commands.Handlers
{
    public class UpdateUserPasswordCommandHandler : IRequestHandler<UpdateUserPasswordCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<UpdateUserPasswordCommandHandler> _logger;
        private readonly UpdateUserPasswordCommandValidator _validator;
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public UpdateUserPasswordCommandHandler(ILogger<UpdateUserPasswordCommandHandler> logger, UpdateUserPasswordCommandValidator validator, IUserRepository userRepository, IAuthenticationContextProvider authenticationContextProvider)
        {
            _logger = logger;
            _validator = validator;
            _userRepository = userRepository;
            _authenticationContextProvider = authenticationContextProvider;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> Handle(UpdateUserPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _userRepository.UpdateUserPassword(request);
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
