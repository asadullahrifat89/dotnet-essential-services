using MediatR;
using Microsoft.Extensions.Logging;
using Identity.Application.DTOs;
using Identity.Application.Queries.Validators;
using Identity.Application.Providers.Interfaces;
using Identity.Domain.Repositories.Interfaces;
using Base.Application.DTOs.Responses;
using Base.Application.Extensions;

namespace Identity.Application.Queries.Handlers
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, QueryRecordResponse<UserResponse>>
    {
        private readonly ILogger<GetUserQueryHandler> _logger;
        private readonly GetUserQueryValidator _validator;
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        public GetUserQueryHandler(
            ILogger<GetUserQueryHandler> logger,
            GetUserQueryValidator validator,
            IUserRepository userRepository,
            IAuthenticationContextProvider authenticationContext)
        {
            _logger = logger;
            _validator = validator;
            _userRepository = userRepository;
            _authenticationContextProvider = authenticationContext;
        }

        public async Task<QueryRecordResponse<UserResponse>> Handle(GetUserQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();
                var result = await _userRepository.GetUser(request.UserId);

                return Response.BuildQueryRecordResponse<UserResponse>().BuildSuccessResponse(UserResponse.Initialize(result), authCtx?.RequestUri);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordResponse<UserResponse>().BuildErrorResponse(
                    Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContextProvider.GetAuthenticationContext().RequestUri));
            }
        }
    }
}
