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
    public class GetProjectsQueryHandler : IRequestHandler<GetProjectsQuery, QueryRecordsResponse<Project>>
    {
        private readonly ILogger<GetProjectsQueryHandler> _logger;
        private readonly GetProjectsQueryValidator _validator;
        private readonly IProjectRepository _projectRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        public GetProjectsQueryHandler(ILogger<GetProjectsQueryHandler> logger, GetProjectsQueryValidator validator, IProjectRepository userRepository, IAuthenticationContextProvider authenticationContextProvider)
        {
            _logger = logger;
            _validator = validator;
            _projectRepository = userRepository;
            _authenticationContextProvider = authenticationContextProvider;
        }

        public async Task<QueryRecordsResponse<Project>> Handle(GetProjectsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();
                var result = await _projectRepository.GetProjects(
                    searchTerm: request.SearchTerm,
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize,
                    publishingStatus: request.PublishingStatus);

                return Response.BuildQueryRecordsResponse<Project>().BuildSuccessResponse(count: result.Count, records: result.Records, authCtx.RequestUri);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordsResponse<Project>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContextProvider.GetAuthenticationContext().RequestUri));
            }
        }
    }
}
