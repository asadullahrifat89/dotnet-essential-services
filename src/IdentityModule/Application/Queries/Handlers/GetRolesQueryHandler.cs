using MediatR;
using Microsoft.Extensions.Logging;
using BaseModule.Infrastructure.Extensions;
using BaseModule.Application.DTOs.Responses;
using IdentityModule.Application.Queries.Validators;
using IdentityModule.Domain.Entities;
using IdentityModule.Domain.Repositories.Interfaces;
using IdentityModule.Application.Providers.Interfaces;

namespace IdentityModule.Application.Queries.Handlers
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
