using MediatR;
using Microsoft.Extensions.Logging;
using Identity.Application.Commands.Validators;
using Base.Application.DTOs.Responses;
using Identity.Domain.Repositories.Interfaces;
using Base.Application.Extensions;
using Identity.Application.Providers.Interfaces;

namespace Identity.Application.Commands.Handlers
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<UpdateRoleCommandHandler> _logger;
        private readonly UpdateRoleCommandValidator _validator;
        private readonly IRoleRepository _roleRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public UpdateRoleCommandHandler(
            ILogger<UpdateRoleCommandHandler> logger,
            UpdateRoleCommandValidator validator,
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

        public async Task<ServiceResponse> Handle(UpdateRoleCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(command, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();
                var result = await _roleRepository.UpdateRole(command.RoleId, command.Claims);

                return Response.BuildServiceResponse().BuildSuccessResponse(result, authCtx.RequestUri);
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
