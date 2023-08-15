using MediatR;
using Microsoft.Extensions.Logging;
using BaseModule.Infrastructure.Extensions;
using BaseModule.Application.DTOs.Responses;
using IdentityModule.Application.Commands.Validators;
using IdentityModule.Domain.Repositories.Interfaces;
using IdentityModule.Application.Providers.Interfaces;

namespace IdentityModule.Application.Commands.Handlers
{
    public class AddRoleCommandHandler : IRequestHandler<AddRoleCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<AddRoleCommandHandler> _logger;
        private readonly AddRoleCommandValidator _validator;
        private readonly IRoleRepository _roleRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public AddRoleCommandHandler(
            ILogger<AddRoleCommandHandler> logger,
            AddRoleCommandValidator validator,
            IRoleRepository roleRepository,
            IAuthenticationContextProvider authenticationContextProvider)
        {
            _logger = logger;
            _validator = validator;
            _roleRepository = roleRepository;
            _authenticationContextProvider = authenticationContextProvider;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> Handle(AddRoleCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(command, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();
                var role = AddRoleCommand.Initialize(command, authCtx);

                return await _roleRepository.AddRole(role, command.Claims);
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
