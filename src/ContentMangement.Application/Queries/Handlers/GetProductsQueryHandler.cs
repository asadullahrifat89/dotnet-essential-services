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
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, QueryRecordsResponse<Product>>
    {

        #region Fields

        private readonly ILogger<GetProductsQueryHandler> _logger;
        private readonly GetProductsQueryValidator _validator;
        private readonly IProductRepository _productRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctors

        public GetProductsQueryHandler(
            ILogger<GetProductsQueryHandler> logger,
            GetProductsQueryValidator validator,
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

        public async Task<QueryRecordsResponse<Product>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();
                var result = await _productRepository.GetProducts(
                    searchTerm: request.SearchTerm,
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize,
                    productCostType: request.ProductCostType,
                    employmentType: request.EmploymentType,
                    publishingStatus: request.PublishingStatus,
                    manPower: request.ManPower,
                    experience: request.Experience);

                return Response.BuildQueryRecordsResponse<Product>().BuildSuccessResponse(count: result.Count, records: result.Records, authCtx.RequestUri);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordsResponse<Product>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContextProvider.GetAuthenticationContext().RequestUri));
            }
        }

        #endregion
    }
}
