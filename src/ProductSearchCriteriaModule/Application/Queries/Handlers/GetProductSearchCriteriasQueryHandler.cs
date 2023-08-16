using BaseModule.Application.DTOs.Responses;
using BaseModule.Infrastructure.Extensions;
using IdentityModule.Application.Providers.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using ProductSearchCriteriaModule.Application.Queries.Validators;
using ProductSearchCriteriaModule.Domain.Entities;
using ProductSearchCriteriaModule.Domain.Repositories.Interfaces;

namespace ProductSearchCriteriaModule.Application.Queries.Handlers
{
    public class GetProductSearchCriteriasQueryHandler : IRequestHandler<GetProductSearchCriteriasQuery, QueryRecordsResponse<ProductSearchCriteria>>
    {
        private readonly ILogger<GetProductSearchCriteriasQueryHandler> _logger;
        private readonly GetProductSearchCriteriasQueryValidator _validator;
        private readonly IProductSearchCriteriaRepository _ProductSearchCriteriaRepository;
        private readonly IAuthenticationContextProvider _authenticationContext;


        public GetProductSearchCriteriasQueryHandler(
            ILogger<GetProductSearchCriteriasQueryHandler> logger,
            GetProductSearchCriteriasQueryValidator validator,
            IProductSearchCriteriaRepository ProductSearchCriteriaRepository,
            IAuthenticationContextProvider authenticationContext)
        {
            _logger = logger;
            _validator = validator;
            _ProductSearchCriteriaRepository = ProductSearchCriteriaRepository;
            _authenticationContext = authenticationContext;
        }

        public async Task<QueryRecordsResponse<ProductSearchCriteria>> Handle(GetProductSearchCriteriasQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _ProductSearchCriteriaRepository.GetProductSearchCriterias(
                    searchTerm: request.SearchTerm,
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize,
                    productSearchCriteriaType: request.ProductSearchCriteriaType,
                    skillsetType: request.SkillsetType);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordsResponse<ProductSearchCriteria>().BuildErrorResponse(
                                       Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContext.GetAuthenticationContext().RequestUri));
            }
        }
    }
}
