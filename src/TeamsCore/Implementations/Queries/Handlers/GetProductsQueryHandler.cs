using BaseCore.Extensions;
using BaseCore.Models.Responses;
using BaseCore.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using TeamsCore.Declarations.Queries;
using TeamsCore.Declarations.Repositories;
using TeamsCore.Implementations.Queries.Validators;
using TeamsCore.Models.Responses;

namespace TeamsCore.Implementations.Queries.Handlers
{
    public class GetProductsQueryHandler : IRequestHandler<GetProductsQuery, QueryRecordsResponse<ProductResponse>>
    {

        #region Fields

        private readonly ILogger<GetProductsQueryHandler> _logger;
        private readonly GetProductsQueryValidator _validator;
        private readonly IProductRepository _productRepository;
        private readonly IAuthenticationContextProvider _authenticationContext;

        #endregion

        #region Ctors

        public GetProductsQueryHandler(
            ILogger<GetProductsQueryHandler> logger,
            GetProductsQueryValidator validator,
            IAuthenticationContextProvider authenticationContext,
            IProductRepository productRepository)
        {
            _logger = logger;
            _validator = validator;
            _authenticationContext = authenticationContext;
            _productRepository = productRepository;
        }

        #endregion

        #region Methods

        public async Task<QueryRecordsResponse<ProductResponse>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();
                
                return await _productRepository.GetProducts(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordsResponse<ProductResponse>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContext.GetAuthenticationContext().RequestUri));
            }
        }

        #endregion
    }
}
