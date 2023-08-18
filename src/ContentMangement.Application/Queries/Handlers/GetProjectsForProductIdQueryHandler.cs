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
    public class GetProjectsForProductIdQueryHandler : IRequestHandler<GetProjectsForProductIdQuery, QueryRecordsResponse<Project>>
    {
        private readonly ILogger<GetProjectsForProductIdQueryHandler> _logger;
        private readonly GetProjectsForProductIdQueryValidator _validator;
        private readonly IProjectRepository _projectRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        public GetProjectsForProductIdQueryHandler(
            ILogger<GetProjectsForProductIdQueryHandler> logger,
            GetProjectsForProductIdQueryValidator validator,
            IProjectRepository userRepository,
            IAuthenticationContextProvider authenticationContextProvider)
        {
            _logger = logger;
            _validator = validator;
            _projectRepository = userRepository;
            _authenticationContextProvider = authenticationContextProvider;
        }

        public async Task<QueryRecordsResponse<Project>> Handle(GetProjectsForProductIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();
                var result = await _projectRepository.GetProjectsForProductId(request.ProductId);

                return Response.BuildQueryRecordsResponse<Project>().BuildSuccessResponse(count: result.Count, records: result.Records, authCtx?.RequestUri);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordsResponse<Project>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContextProvider.GetAuthenticationContext().RequestUri));
            }
        }
    }
}
