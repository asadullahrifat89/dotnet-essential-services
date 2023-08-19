using MediatR;
using Microsoft.Extensions.Logging;
using Identity.Application.Queries.Validators;
using Identity.Application.DTOs;
using Identity.Application.Providers.Interfaces;
using Base.Application.DTOs.Responses;
using Identity.Domain.Repositories.Interfaces;
using Base.Application.Extensions;

namespace Identity.Application.Queries.Handlers
{
    public class GetUsersQueryHandler : IRequestHandler<GetUsersQuery, QueryRecordsResponse<UserResponse>>
    {
        private readonly ILogger<GetUsersQueryHandler> _logger;
        private readonly GetUsersQueryValidator _validator;
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        public GetUsersQueryHandler(ILogger<GetUsersQueryHandler> logger, GetUsersQueryValidator validator, IUserRepository userRepository, IAuthenticationContextProvider authenticationContext)
        {
            _logger = logger;
            _validator = validator;
            _userRepository = userRepository;
            _authenticationContextProvider = authenticationContext;
        }

        public async Task<QueryRecordsResponse<UserResponse>> Handle(GetUsersQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();
                var result = await _userRepository.GetUsers(
                    searchTerm: request.SearchTerm,
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize);

                return new QueryRecordsResponse<UserResponse>().BuildSuccessResponse(
                   count: result.Count,
                   records: result.Users is not null ? result.Users.Select(x => UserResponse.Map(x)).ToArray() : Array.Empty<UserResponse>(), authCtx?.RequestUri);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordsResponse<UserResponse>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContextProvider.GetAuthenticationContext().RequestUri));
            }
        }
    }
}
