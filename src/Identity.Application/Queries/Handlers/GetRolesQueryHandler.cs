using MediatR;
using Microsoft.Extensions.Logging;
using Identity.Application.Queries.Validators;
using Identity.Application.Providers.Interfaces;
using Base.Application.DTOs.Responses;
using Identity.Domain.Repositories.Interfaces;
using Identity.Domain.Entities;
using Base.Application.Extensions;

namespace Identity.Application.Queries.Handlers
{
    public class GetRolesQueryHandler : IRequestHandler<GetRolesQuery, QueryRecordsResponse<Role>>
    {
        private readonly ILogger<GetRolesQueryHandler> _logger;
        private readonly GetRolesQueryValidator _validator;
        private readonly IRoleRepository _roleRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        public GetRolesQueryHandler(
            ILogger<GetRolesQueryHandler> logger,
            GetRolesQueryValidator validator,
            IRoleRepository roleRepository,
            IAuthenticationContextProvider authenticationContext)
        {
            _logger = logger;
            _validator = validator;
            _roleRepository = roleRepository;
            _authenticationContextProvider = authenticationContext;
        }

        public async Task<QueryRecordsResponse<Role>> Handle(GetRolesQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();
                var result = await _roleRepository.GetRoles(
                    searchTerm: request.SearchTerm,
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize);

                return new QueryRecordsResponse<Role>().BuildSuccessResponse(
                   count: result.Count,
                   records: result.Roles, authCtx?.RequestUri);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordsResponse<Role>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContextProvider.GetAuthenticationContext().RequestUri));
            }

        }
    }
}
