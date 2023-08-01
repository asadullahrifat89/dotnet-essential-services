using IdentityCore.Contracts.Declarations.Commands;
using IdentityCore.Contracts.Declarations.Queries;
using IdentityCore.Contracts.Declarations.Repositories;
using IdentityCore.Contracts.Implementations.Commands.Handlers;
using IdentityCore.Contracts.Implementations.Commands.Validators;
using IdentityCore.Contracts.Implementations.Queries.Validators;
using IdentityCore.Extensions;
using IdentityCore.Models.Responses;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Contracts.Implementations.Queries.Handlers
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, QueryRecordResponse<UserResponse>>
    {
        private readonly ILogger<GetUserQueryHandler> _logger;
        private readonly GetUserQueryValidator _validator;
        private readonly IUserRepository _userRepository;

        public GetUserQueryHandler(ILogger<GetUserQueryHandler> logger, GetUserQueryValidator validator, IUserRepository userRepository)
        {
            _logger = logger;
            _validator = validator;
            _userRepository = userRepository;
        }

        public async Task<QueryRecordResponse<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _userRepository.GetUser(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordResponse<UserResponse>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError(ex.Message));
            }
        }
    }
}
