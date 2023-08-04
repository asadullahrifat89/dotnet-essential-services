using BaseCore.Models.Responses;
using IdentityCore.Declarations.Queries;
using IdentityCore.Declarations.Repositories;
using BaseCore.Extensions;
using IdentityCore.Implementations.Queries.Validators;
using MediatR;
using Microsoft.Extensions.Logging;
using BaseCore.Services;

namespace IdentityCore.Implementations.Queries.Handlers
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, QueryRecordResponse<UserResponse>>
    {
        private readonly ILogger<GetUserQueryHandler> _logger;
        private readonly GetUserQueryValidator _validator;
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationContextProvider _authenticationContext;

        public GetUserQueryHandler(
            ILogger<GetUserQueryHandler> logger,
            GetUserQueryValidator validator,
            IUserRepository userRepository,
            IAuthenticationContextProvider authenticationContext)
        {
            _logger = logger;
            _validator = validator;
            _userRepository = userRepository;
            _authenticationContext = authenticationContext;
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
                return Response.BuildQueryRecordResponse<UserResponse>().BuildErrorResponse(
                    Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContext.GetAuthenticationContext().RequestUri));
            }
        }
    }
}
