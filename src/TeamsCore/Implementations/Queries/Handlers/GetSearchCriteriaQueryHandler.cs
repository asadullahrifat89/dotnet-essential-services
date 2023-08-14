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
    public class GetSearchCriteriaQueryHandler : IRequestHandler<GetSearchCriteriaQuery, QueryRecordResponse<SearchCriteria>>
    {
        #region Fields

        private readonly ILogger<GetSearchCriteriaQueryHandler> _logger;
        private readonly GetSearchCriteriaQueryValidator _validator;
        private readonly ISearchCriteriaRepository _searchCriteriaRepository;
        private readonly IAuthenticationContextProvider _authenticationContext;

        #endregion

        #region Ctors

        public GetSearchCriteriaQueryHandler(
            ILogger<GetSearchCriteriaQueryHandler> logger,
            GetSearchCriteriaQueryValidator validator,
            ISearchCriteriaRepository searchCriteriaRepository,
            IAuthenticationContextProvider authenticationContext)
        {
            _logger = logger;
            _validator = validator;
            _searchCriteriaRepository = searchCriteriaRepository;
            _authenticationContext = authenticationContext;
        }

        #endregion

        public async Task<QueryRecordResponse<SearchCriteria>> Handle(GetSearchCriteriaQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _searchCriteriaRepository.GetSearchCriteria(request);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordResponse<SearchCriteria>().BuildErrorResponse(
                    Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContext.GetAuthenticationContext().RequestUri));
            }
        }
    }
}
