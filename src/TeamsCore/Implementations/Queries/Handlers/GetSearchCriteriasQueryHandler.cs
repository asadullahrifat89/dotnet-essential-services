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
    public class GetSearchCriteriasQueryHandler : IRequestHandler<GetSearchCriteriasQuery, QueryRecordsResponse<SearchCriteria>>
    {
        private readonly ILogger<GetSearchCriteriasQueryHandler> _logger;
        private readonly GetSearchCriteriasQueryValidator _validator;
        private readonly ISearchCriteriaRepository _searchCriteriaRepository;
        private readonly IAuthenticationContextProvider _authenticationContext;


        public GetSearchCriteriasQueryHandler(
            ILogger<GetSearchCriteriasQueryHandler> logger,
            GetSearchCriteriasQueryValidator validator,
            ISearchCriteriaRepository searchCriteriaRepository,
            IAuthenticationContextProvider authenticationContext)
        {
            _logger = logger;
            _validator = validator;
            _searchCriteriaRepository = searchCriteriaRepository;
            _authenticationContext = authenticationContext;
        }

        public async Task<QueryRecordsResponse<SearchCriteria>> Handle(GetSearchCriteriasQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _searchCriteriaRepository.GetSearchCriterias(request);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordsResponse<SearchCriteria>().BuildErrorResponse(
                                       Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContext.GetAuthenticationContext().RequestUri));
            }
        }
    }
}
