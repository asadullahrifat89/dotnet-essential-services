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
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, QueryRecordResponse<Product>>
    {
        private readonly ILogger<GetProductQueryHandler> _logger;
        private readonly GetProductQueryValidator _validator;
        private readonly IProductRepository _productRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        public GetProductQueryHandler(
            ILogger<GetProductQueryHandler> logger,
            GetProductQueryValidator validator,
            IProductRepository productRepository,
            IAuthenticationContextProvider authenticationContextProvider)
        {
            _logger = logger;
            _validator = validator;
            _productRepository = productRepository;
            _authenticationContextProvider = authenticationContextProvider;
        }


        public async Task<QueryRecordResponse<Product>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();
                var result = await _productRepository.GetProduct(request.ProductId);

                return Response.BuildQueryRecordResponse<Product>().BuildSuccessResponse(result, authCtx.RequestUri);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordResponse<Product>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContextProvider.GetAuthenticationContext().RequestUri));
            }
        }
    }
}
