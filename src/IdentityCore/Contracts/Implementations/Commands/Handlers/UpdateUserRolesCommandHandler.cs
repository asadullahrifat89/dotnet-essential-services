using IdentityCore.Contracts.Declarations.Commands;
using IdentityCore.Contracts.Declarations.Repositories;
using IdentityCore.Contracts.Implementations.Commands.Validators;
using IdentityCore.Extensions;
using IdentityCore.Models.Responses;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Contracts.Implementations.Commands.Handlers
{
    public class UpdateUserRolesCommandHandler : IRequestHandler<UpdateUserRolesCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<UpdateUserRolesCommandHandler> _logger;
        private readonly UpdateUserRolesCommandValidator _validator;
        private readonly IUserRepository _userRepository;

        #endregion

        #region Ctor

        public UpdateUserRolesCommandHandler(ILogger<UpdateUserRolesCommandHandler> logger, UpdateUserRolesCommandValidator validator, IUserRepository userRepository)
        {
            _logger = logger;
            _validator = validator;
            _userRepository = userRepository;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> Handle(UpdateUserRolesCommand request, CancellationToken cancellationToken)
        {
            
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _userRepository.UpdateUserRoles(request);
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
