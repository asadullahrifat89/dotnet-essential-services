using IdentityCore.Declarations.Commands;
using IdentityCore.Declarations.Repositories;
using IdentityCore.Extensions;
using IdentityCore.Implementations.Commands.Validators;
using IdentityCore.Models.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityCore.Implementations.Commands.Handlers
{
    public class UpdateRoleCommandHandler : IRequestHandler<UpdateRoleCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<UpdateRoleCommandHandler> _logger;
        private readonly UpdateRoleCommandValidator _validator;
        private readonly IRoleRepository _roleRepository;

        #endregion

        #region Ctor

        public UpdateRoleCommandHandler(ILogger<UpdateRoleCommandHandler> logger, UpdateRoleCommandValidator validator, IRoleRepository roleRepository)
        {
            _logger = logger;
            _validator = validator;
            _roleRepository = roleRepository;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> Handle(UpdateRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _roleRepository.UpdateRole(request);
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
