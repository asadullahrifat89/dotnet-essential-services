using BaseCore.Declarations.Services;
using BaseCore.Models.Responses;
using IdentityCore.Declarations.Queries;
using IdentityCore.Declarations.Repositories;
using BaseCore.Extensions;
using IdentityCore.Implementations.Queries.Validators;
using IdentityCore.Models.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityCore.Implementations.Queries.Handlers
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
