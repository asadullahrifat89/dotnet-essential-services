using MediatR;
using Microsoft.Extensions.Logging;
using IdentityModule.Implementations.Queries.Validators;
using BaseModule.Models.Responses;
using BaseModule.Services;
using IdentityModule.Declarations.Queries;
using IdentityModule.Models.Entities;
using IdentityModule.Declarations.Repositories;
using BaseModule.Extensions;

namespace IdentityModule.Implementations.Queries.Handlers
{
    public class GetClaimsQueryHandler : IRequestHandler<GetClaimsQuery, QueryRecordsResponse<ClaimPermission>>
    {
        private readonly ILogger<GetRolesQueryHandler> _logger;
        private readonly GetClaimsQueryValidator _validator;
        private readonly IClaimPermissionRepository _claimPermissionRepository;
        private readonly IAuthenticationContextProvider _authenticationContext;

        public GetClaimsQueryHandler(ILogger<GetRolesQueryHandler> logger, GetClaimsQueryValidator validator, IClaimPermissionRepository claimPermissionRepository, IAuthenticationContextProvider authenticationContext)
        {
            _logger = logger;
            _validator = validator;
            _claimPermissionRepository = claimPermissionRepository;
            _authenticationContext = authenticationContext;
        }

        public async Task<QueryRecordsResponse<ClaimPermission>> Handle(GetClaimsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _claimPermissionRepository.GetClaims(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordsResponse<ClaimPermission>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContext.GetAuthenticationContext().RequestUri));
            }

        }
    }
}
