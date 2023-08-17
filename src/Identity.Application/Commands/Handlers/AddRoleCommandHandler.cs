using MediatR;
using Microsoft.Extensions.Logging;
using Identity.Application.Commands.Validators;
using Identity.Application.Providers.Interfaces;
using Base.Application.DTOs.Responses;
using Base.Application.Extensions;
using Identity.Domain.Repositories.Interfaces;

namespace Identity.Application.Commands.Handlers
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

                var result = await _roleRepository.AddRole(role, command.Claims);

                return Response.BuildServiceResponse().BuildSuccessResponse(result, authCtx?.RequestUri);
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
