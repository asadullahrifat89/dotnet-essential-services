using IdentityCore.Contracts.Declarations.Queries;
using IdentityCore.Contracts.Declarations.Repositories;
using IdentityCore.Contracts.Declarations.Services;
using IdentityCore.Contracts.Implementations.Queries.Validators;
using IdentityCore.Extensions;
using IdentityCore.Models.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityCore.Contracts.Implementations.Queries.Handlers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, QueryRecordsResponse<UserResponse>>
    {
        private readonly ILogger<GetUsersQueryHandler> _logger;
        private readonly GetUsersQueryValidator _validator;
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationContext _authenticationContext;

        public GetUsersQueryHandler(ILogger<GetUsersQueryHandler> logger, GetUsersQueryValidator validator, IUserRepository userRepository, IAuthenticationContext authenticationContext)
        {
            _logger = logger;
            _validator = validator;
            _userRepository = userRepository;
            _authenticationContext = authenticationContext;
        }

        public async Task<QueryRecordsResponse<UserResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _userRepository.GetUsers(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordsResponse<UserResponse>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContext.GetAuthenticationContext().RequestUri));
            }
        }
    }
}
