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
    public class GetProductSearchCriteriasQueryHandler : IRequestHandler<GetProductSearchCriteriasQuery, QueryRecordsResponse<ProductSearchCriteria>>
    {
        private readonly ILogger<GetProductSearchCriteriasQueryHandler> _logger;
        private readonly GetProductSearchCriteriasQueryValidator _validator;
        private readonly IProductSearchCriteriaRepository _ProductSearchCriteriaRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;


        public GetProductSearchCriteriasQueryHandler(
            ILogger<GetProductSearchCriteriasQueryHandler> logger,
            GetProductSearchCriteriasQueryValidator validator,
            IProductSearchCriteriaRepository ProductSearchCriteriaRepository,
            IAuthenticationContextProvider authenticationContextProvider)
        {
            _logger = logger;
            _validator = validator;
            _ProductSearchCriteriaRepository = ProductSearchCriteriaRepository;
            _authenticationContextProvider = authenticationContextProvider;
        }

        public async Task<QueryRecordsResponse<ProductSearchCriteria>> Handle(GetProductSearchCriteriasQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();
                var result = await _ProductSearchCriteriaRepository.GetProductSearchCriterias(
                    searchTerm: request.SearchTerm,
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize,
                    //searchCriteriaType: request.SearchCriteriaType,
                    skillsetType: request.SkillsetType);

                return Response.BuildQueryRecordsResponse<ProductSearchCriteria>().BuildSuccessResponse(result.Count, result.Records, authCtx.RequestUri);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordsResponse<ProductSearchCriteria>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContextProvider.GetAuthenticationContext().RequestUri));
            }
        }
    }
}
