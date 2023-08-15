using MediatR;
using Microsoft.Extensions.Logging;
using BaseModule.Infrastructure.Extensions;
using BaseModule.Application.DTOs.Responses;
using IdentityModule.Application.Commands.Validators;
using IdentityModule.Domain.Repositories.Interfaces;
using IdentityModule.Application.Providers.Interfaces;

namespace IdentityModule.Application.Commands.Handlers
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

        public SubmitUserCommandHandler(ILogger<SubmitUserCommandHandler> logger, SubmitUserCommandValidator validator, IUserRepository userRepository, IAuthenticationContextProvider authenticationContextProvider)
        {
            _logger = logger;
            _validator = validator;
            _userRepository = userRepository;
            _authenticationContextProvider = authenticationContextProvider;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> Handle(SubmitUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _userRepository.SubmitUser(request);
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
