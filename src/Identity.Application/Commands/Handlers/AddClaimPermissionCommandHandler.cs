using MediatR;
using Microsoft.Extensions.Logging;
using Identity.Application.Commands.Validators;
using Identity.Application.Providers.Interfaces;
using Base.Application.DTOs.Responses;
using Identity.Domain.Repositories.Interfaces;
using Base.Application.Extensions;

namespace Identity.Application.Commands.Handlers
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
                var claimPermission = AddClaimPermissionCommand.Map(command, authCtx);

                var result = await _claimPermissionRepository.AddClaimPermission(claimPermission);

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
