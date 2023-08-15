﻿using MediatR;
using Microsoft.Extensions.Logging;
using IdentityModule.Declarations.Repositories;
using IdentityModule.Declarations.Commands;
using IdentityModule.Implementations.Commands.Validators;
using BaseModule.Infrastructure.Extensions;
using BaseModule.Application.DTOs.Responses;

namespace IdentityModule.Implementations.Commands.Handlers
{
    public class UpdateUserCommandHandler : IRequestHandler<UpdateUserCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<UpdateUserCommandHandler> _logger;
        private readonly UpdateUserCommandValidator _validator;
        private readonly IUserRepository _userRepository;

        #endregion

        #region Ctor

        public UpdateUserCommandHandler(ILogger<UpdateUserCommandHandler> logger, UpdateUserCommandValidator validator, IUserRepository userRepository)
        {
            _logger = logger;
            _validator = validator;
            _userRepository = userRepository;
        }

        #endregion

        #region Methods

        public async Task<ServiceResponse> Handle(UpdateUserCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _userRepository.UpdateUser(request);
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
