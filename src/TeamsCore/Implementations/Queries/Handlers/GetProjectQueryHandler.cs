using BaseCore.Extensions;
using BaseCore.Models.Responses;
using BaseCore.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using TeamsCore.Declarations.Queries;
using TeamsCore.Declarations.Repositories;
using TeamsCore.Implementations.Queries.Validators;
using TeamsCore.Models.Entities;

namespace TeamsCore.Implementations.Queries.Handlers
{
    public class GetProjectQueryHandler : IRequestHandler<GetProjectQuery, QueryRecordResponse<Project>>
    {
        private readonly ILogger<GetProjectQueryHandler> _logger;
        private readonly GetProjectQueryValidator _validator;
        private readonly IProjectRepository _projectRepository;
        private readonly IAuthenticationContextProvider _authenticationContext;

        public GetProjectQueryHandler(
            ILogger<GetProjectQueryHandler> logger,
            GetProjectQueryValidator validator,
            IProjectRepository projectRepository,
            IAuthenticationContextProvider authenticationContext)
        {
            _logger = logger;
            _validator = validator;
            _projectRepository = projectRepository;
            _authenticationContext = authenticationContext;
        }


        public async Task<QueryRecordResponse<Project>> Handle(GetProjectQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _projectRepository.GetProject(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordResponse<Project>().BuildErrorResponse(
                    Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContext.GetAuthenticationContext().RequestUri));
            }
        }
    }

}
