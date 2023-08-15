using MediatR;
using Microsoft.Extensions.Logging;
using BaseModule.Infrastructure.Extensions;
using BaseModule.Application.DTOs.Responses;
using IdentityModule.Application.Commands.Validators;
using IdentityModule.Domain.Repositories.Interfaces;
using MongoDB.Driver;
using IdentityModule.Application.Providers.Interfaces;

namespace IdentityModule.Application.Commands.Handlers
{
    public class AddClaimPermissionCommandHandler : IRequestHandler<AddClaimPermissionCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<AddClaimPermissionCommandHandler> _logger;
        private readonly AddClaimPermissionCommandValidator _validator;
        private readonly IClaimPermissionRepository _claimPermissionRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public AddClaimPermissionCommandHandler(
            ILogger<AddClaimPermissionCommandHandler> logger,
            AddClaimPermissionCommandValidator validator,
            IClaimPermissionRepository claimPermissionRepository,
            IAuthenticationContextProvider authenticationContextProvider)
        {
            _logger = logger;
            _validator = validator;
            _claimPermissionRepository = claimPermissionRepository;
            _authenticationContextProvider = authenticationContextProvider;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> Handle(AddClaimPermissionCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(command, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();
                var claimPermission = AddClaimPermissionCommand.Initialize(command, authCtx);

                return await _claimPermissionRepository.AddClaimPermission(claimPermission);
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
