using MediatR;
using Microsoft.Extensions.Logging;
using Identity.Application.Queries.Validators;
using Identity.Application.Providers.Interfaces;
using Identity.Domain.Repositories.Interfaces;
using Base.Application.DTOs.Responses;
using Identity.Domain.Entities;
using Base.Application.Extensions;

namespace Identity.Application.Queries.Handlers
{
    public class GetUserRolesQueryHandler : IRequestHandler<GetUserRolesQuery, QueryRecordsResponse<Role>>
    {
        private readonly ILogger<GetRolesQueryHandler> _logger;
        private readonly GetUserRolesQueryValidator _validator;
        private readonly IRoleRepository _roleRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        public GetUserRolesQueryHandler(
            ILogger<GetRolesQueryHandler> logger,
            GetUserRolesQueryValidator validator,
            IRoleRepository roleRepository,
            IAuthenticationContextProvider authenticationContext)
        {
            _logger = logger;
            _validator = validator;
            _roleRepository = roleRepository;
            _authenticationContextProvider = authenticationContext;
        }

        public async Task<QueryRecordsResponse<Role>> Handle(GetUserRolesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();
                var result = await _roleRepository.GetRolesByUserId(request.UserId);

                return Response.BuildQueryRecordsResponse<Role>().BuildSuccessResponse(
                       count: result.Count,
                       records: result.Roles, authCtx.RequestUri);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordsResponse<Role>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContextProvider.GetAuthenticationContext().RequestUri));
            }

        }
    }
}
