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
    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, QueryRecordsResponse<Role>>
    {
        private readonly ILogger<GetRolesQueryHandler> _logger;
        private readonly GetRolesQueryValidator _validator;
        private readonly IRoleRepository _roleRepository;
        private readonly IAuthenticationContextProvider _authenticationContext;

        public GetRolesQueryHandler(ILogger<GetRolesQueryHandler> logger, GetRolesQueryValidator validator, IRoleRepository roleRepository, IAuthenticationContextProvider authenticationContext)
        {
            _logger = logger;
            _validator = validator;
            _roleRepository = roleRepository;
            _authenticationContext = authenticationContext;
        }

        public async Task<QueryRecordsResponse<Role>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _roleRepository.GetRoles(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordsResponse<Role>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContext.GetAuthenticationContext().RequestUri));
            }

        }
    }
}
