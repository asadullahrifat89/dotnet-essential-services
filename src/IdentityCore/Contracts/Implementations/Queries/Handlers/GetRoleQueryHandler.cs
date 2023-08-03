using FluentValidation;
using IdentityCore.Contracts.Declarations.Queries;
using IdentityCore.Contracts.Declarations.Repositories;
using IdentityCore.Contracts.Declarations.Services;
using IdentityCore.Contracts.Implementations.Queries.Validators;
using IdentityCore.Extensions;
using IdentityCore.Models.Entities;
using IdentityCore.Models.Responses;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore.Contracts.Implementations.Queries.Handlers
{
    public class GetRoleQueryHandler : IRequestHandler<GetRoleQuery, QueryRecordsResponse<Role>>
    {
        private readonly ILogger<GetRolesQueryHandler> _logger;
        private readonly GetRoleQueryValidator _validator;
        private readonly IRoleRepository _roleRepository;
        private readonly IAuthenticationContextProvider _authenticationContext;

        public GetRoleQueryHandler(ILogger<GetRolesQueryHandler> logger, GetRoleQueryValidator validator, IRoleRepository roleRepository, IAuthenticationContextProvider authenticationContext)
        {
            _logger = logger;
            _validator = validator;
            _roleRepository = roleRepository;
            _authenticationContext = authenticationContext;
        }

        public async Task<QueryRecordsResponse<Role>> Handle(GetRoleQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _roleRepository.GetRoleByUserId(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordsResponse<Role>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContext.GetAuthenticationContext().RequestUri));
            }

        }
    }
}
