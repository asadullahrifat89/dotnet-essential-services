using MediatR;
using Microsoft.Extensions.Logging;
using IdentityModule.Declarations.Queries;
using IdentityModule.Declarations.Repositories;
using IdentityModule.Models.Entities;
using IdentityModule.Implementations.Queries.Validators;
using BaseModule.Infrastructure.Extensions;
using BaseModule.Infrastructure.Services.Interfaces;
using BaseModule.Application.DTOs.Responses;

namespace IdentityModule.Implementations.Queries.Handlers
{
    public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, QueryRecordsResponse<Role>>
    {
        private readonly ILogger<GetRolesQueryHandler> _logger;
        private readonly GetUserRolesQueryValidator _validator;
        private readonly IRoleRepository _roleRepository;
        private readonly IAuthenticationContextProviderService _authenticationContext;

        public GetUserRolesQueryHandler(ILogger<GetRolesQueryHandler> logger, GetUserRolesQueryValidator validator, IRoleRepository roleRepository, IAuthenticationContextProviderService authenticationContext)
        {
            _logger = logger;
            _validator = validator;
            _roleRepository = roleRepository;
            _authenticationContext = authenticationContext;
        }

        public async Task<QueryRecordsResponse<Role>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _roleRepository.GetRolesByUserId(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordsResponse<Role>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContext.GetAuthenticationContext().RequestUri));
            }

        }
    }
}
