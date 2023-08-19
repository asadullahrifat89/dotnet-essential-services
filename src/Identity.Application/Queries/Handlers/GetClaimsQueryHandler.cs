using MediatR;
using Microsoft.Extensions.Logging;
using Identity.Application.Queries.Validators;
using Identity.Application.Providers.Interfaces;
using Base.Application.DTOs.Responses;
using Identity.Domain.Entities;
using Identity.Domain.Repositories.Interfaces;
using Base.Application.Extensions;

namespace Identity.Application.Queries.Handlers
{
    public class GetClaimsQueryHandler : IRequestHandler<GetClaimsQuery, QueryRecordsResponse<ClaimPermission>>
    {
        private readonly ILogger<GetRolesQueryHandler> _logger;
        private readonly GetClaimsQueryValidator _validator;
        private readonly IClaimPermissionRepository _claimPermissionRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        public GetClaimsQueryHandler(ILogger<GetRolesQueryHandler> logger, GetClaimsQueryValidator validator, IClaimPermissionRepository claimPermissionRepository, IAuthenticationContextProvider authenticationContext)
        {
            _logger = logger;
            _validator = validator;
            _claimPermissionRepository = claimPermissionRepository;
            _authenticationContextProvider = authenticationContext;
        }

        public async Task<QueryRecordsResponse<ClaimPermission>> Handle(GetClaimsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();
                var result = await _claimPermissionRepository.GetClaims();

                return Response.BuildQueryRecordsResponse<ClaimPermission>().BuildSuccessResponse(
                   count: result.Count,
                   records: result.ClaimPermissions, requestUri: authCtx.RequestUri);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordsResponse<ClaimPermission>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContextProvider.GetAuthenticationContext().RequestUri));
            }

        }
    }
}
