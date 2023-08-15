using MediatR;
using Microsoft.Extensions.Logging;
using IdentityModule.Declarations.Queries;
using BaseModule.Models.Responses;
using BaseModule.Services;
using IdentityModule.Declarations.Repositories;
using IdentityModule.Implementations.Queries.Validators;
using IdentityModule.Models.Entities;
using BaseModule.Extensions;

namespace IdentityModule.Implementations.Queries.Handlers
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
