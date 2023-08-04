using BaseCore.Models.Responses;
using IdentityCore.Declarations.Commands;
using IdentityCore.Declarations.Repositories;
using BaseCore.Extensions;
using IdentityCore.Implementations.Commands.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityCore.Implementations.Commands.Handlers
{
    public class AddRoleCommandHandler : IRequestHandler<AddRoleCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<AddRoleCommandHandler> _logger;
        private readonly AddRoleCommandValidator _validator;
        private readonly IRoleRepository _roleRepository;

        #endregion

        #region Ctor

        public AddRoleCommandHandler(ILogger<AddRoleCommandHandler> logger, AddRoleCommandValidator validator, IRoleRepository roleRepository)
        {
            _logger = logger;
            _validator = validator;
            _roleRepository = roleRepository;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> Handle(AddRoleCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _roleRepository.AddRole(request);
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
