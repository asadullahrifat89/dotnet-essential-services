using Base.Application.DTOs.Responses;
using Base.Application.Extensions;
using Identity.Application.Providers.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Teams.ContentMangement.Application.DTOs.Responses;
using Teams.ContentMangement.Domain.Repositories.Interfaces;
using Teams.CustomerEngagement.Application.Queries.Validators;

namespace Teams.CustomerEngagement.Application.Queries.Handlers
{
    public class GetProductRecommendationsQueryHandler : IRequestHandler<GetProductRecommendationsQuery, QueryRecordsResponse<ProductRecommendation>>
    {
        #region Fields

        private readonly ILogger<GetProductRecommendationsQueryHandler> _logger;
        private readonly GetProductRecommendationsQueryValidator _validator;
        private readonly IProductRepository _productRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctors

        public GetProductRecommendationsQueryHandler(
            ILogger<GetProductRecommendationsQueryHandler> logger,
            GetProductRecommendationsQueryValidator validator,
            IAuthenticationContextProvider authenticationContextProvider,
            IProductRepository productRepository)
        {
            _logger = logger;
            _validator = validator;
            _authenticationContextProvider = authenticationContextProvider;
            _productRepository = productRepository;
        }

        #endregion

        #region Methods

        public async Task<QueryRecordsResponse<ProductRecommendation>> Handle(GetProductRecommendationsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();
                var result = await _productRepository.GetProductRecommendations(
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize,
                    productSearchCriteriaIds: request.ProductSearchCriteriaIds,
                    employmentTypes: request.EmploymentTypes,
                    manPower: request.MinimumManPower,
                    experience: request.MinimumExperience);

                var records = result.Records.Select(x => ProductRecommendation.Initialize(x)).ToArray();

                return Response.BuildQueryRecordsResponse<ProductRecommendation>().BuildSuccessResponse(count: result.Count, records: records, authCtx?.RequestUri);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordsResponse<ProductRecommendation>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContextProvider.GetAuthenticationContext().RequestUri));
            }
        }

        #endregion
    }
}
