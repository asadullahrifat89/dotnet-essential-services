using MediatR;
using Microsoft.Extensions.Logging;
using BaseModule.Infrastructure.Extensions;
using BaseModule.Application.DTOs.Responses;
using IdentityModule.Domain.Repositories.Interfaces;
using IdentityModule.Application.Queries.Validators;
using IdentityModule.Application.DTOs;
using IdentityModule.Infrastructure.Services.Interfaces;

namespace IdentityModule.Application.Queries.Handlers
{
    public class GetUserQueryHandler : IRequestHandler<GetUserQuery, QueryRecordResponse<UserResponse>>
    {
        private readonly ILogger<GetUserQueryHandler> _logger;
        private readonly GetUserQueryValidator _validator;
        private readonly IUserRepository _userRepository;
        private readonly IAuthenticationContextProviderService _authenticationContext;

        public GetUserQueryHandler(
            ILogger<GetUserQueryHandler> logger,
            GetUserQueryValidator validator,
            IUserRepository userRepository,
            IAuthenticationContextProviderService authenticationContext)
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
