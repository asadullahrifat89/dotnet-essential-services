using BaseCore.Extensions;
using BaseCore.Models.Responses;
using BaseCore.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsCore.Declarations.Queries;
using TeamsCore.Declarations.Repositories;
using TeamsCore.Implementations.Queries.Validators;
using TeamsCore.Models.Entities;

namespace TeamsCore.Implementations.Queries.Handlers
{
    public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, QueryRecordsResponse<Project>>
    {

        private readonly ILogger<GetProjectsQueryHandler> _logger;
        private readonly GetProjectsQueryValidator _validator;
        private readonly IProjectRepository _projectRepository;
        private readonly IAuthenticationContextProvider _authenticationContext;

        public GetProjectsQueryHandler(ILogger<GetProjectsQueryHandler> logger, GetProjectsQueryValidator validator, IProjectRepository userRepository, IAuthenticationContextProvider authenticationContext)
        {
            _logger = logger;
            _validator = validator;
            _projectRepository = userRepository;
            _authenticationContext = authenticationContext;
        }

        public async Task<QueryRecordsResponse<Project>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _projectRepository.GetProjects(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordsResponse<Project>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContext.GetAuthenticationContext().RequestUri));
            }
        }
    }
}
