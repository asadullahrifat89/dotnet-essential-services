using IdentityCore.Declarations.Queries;
using IdentityCore.Declarations.Repositories;
using IdentityCore.Declarations.Services;
using IdentityCore.Extensions;
using IdentityCore.Implementations.Queries.Validators;
using IdentityCore.Models.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityCore.Implementations.Queries.Handlers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, QueryRecordsResponse<UserResponse>>
    {
        private readonly ILogger<GetUsersQueryHandler> _logger;
        private readonly GetUsersQueryValidator _validator;
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationContextProvider _authenticationContext;

        public GetUsersQueryHandler(ILogger<GetUsersQueryHandler> logger, GetUsersQueryValidator validator, IUserRepository userRepository, IAuthenticationContextProvider authenticationContext)
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
