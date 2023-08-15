using MediatR;
using Microsoft.Extensions.Logging;
using BaseModule.Infrastructure.Extensions;
using BaseModule.Application.DTOs.Responses;
using IdentityModule.Application.Commands.Validators;
using IdentityModule.Domain.Repositories.Interfaces;

namespace IdentityModule.Application.Commands.Handlers
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
