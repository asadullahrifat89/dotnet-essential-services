using MediatR;
using Microsoft.Extensions.Logging;
using IdentityModule.Implementations.Commands.Validators;
using BaseModule.Models.Responses;
using IdentityModule.Declarations.Commands;
using IdentityModule.Declarations.Repositories;
using BaseModule.Extensions;

namespace IdentityModule.Implementations.Commands.Handlers
{
    public class AddClaimPermissionCommandHandler : IRequestHandler<AddClaimPermissionCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<AddClaimPermissionCommandHandler> _logger;
        private readonly AddClaimPermissionCommandValidator _validator;
        private readonly IClaimPermissionRepository _claimPermissionRepository;

        #endregion

        #region Ctor

        public AddClaimPermissionCommandHandler(
            ILogger<AddClaimPermissionCommandHandler> logger,
            AddClaimPermissionCommandValidator validator,
            IClaimPermissionRepository claimPermissionRepository)
        {
            _logger = logger;
            _validator = validator;
            _claimPermissionRepository = claimPermissionRepository;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> Handle(AddClaimPermissionCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _claimPermissionRepository.AddClaimPermission(request);
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
