using Base.Application.DTOs.Responses;
using Base.Application.Extensions;
using Identity.Application.Providers.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Teams.ContentMangement.Application.Queries.Validators;
using Teams.ContentMangement.Domain.Entities;
using Teams.ContentMangement.Domain.Repositories.Interfaces;

namespace Teams.ContentMangement.Application.Queries.Handlers
{
    public class GetProjectQueryHandler : IRequestHandler<GetProjectQuery, QueryRecordResponse<Project>>
    {
        private readonly ILogger<GetProjectQueryHandler> _logger;
        private readonly GetProjectQueryValidator _validator;
        private readonly IProjectRepository _projectRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        public GetProjectQueryHandler(
            ILogger<GetProjectQueryHandler> logger,
            GetProjectQueryValidator validator,
            IProjectRepository projectRepository,
            IAuthenticationContextProvider authenticationContextProvider)
        {
            _logger = logger;
            _validator = validator;
            _projectRepository = projectRepository;
            _authenticationContextProvider = authenticationContextProvider;
        }


        public async Task<QueryRecordResponse<Project>> Handle(GetProjectQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();
                var result = await _projectRepository.GetProject(request.ProjectId);

                return Response.BuildQueryRecordResponse<Project>().BuildSuccessResponse(result, authCtx.RequestUri);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordResponse<Project>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContextProvider.GetAuthenticationContext().RequestUri));
            }
        }
    }
}
