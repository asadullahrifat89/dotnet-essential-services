using BaseCore.Extensions;
using BaseCore.Models.Responses;
using BaseCore.Services;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsCore.Declarations.Queries;
using TeamsCore.Declarations.Repositories;
using TeamsCore.Implementations.Queries.Validators;
using TeamsCore.Implementations.Repositories;
using TeamsCore.Models.Entities;
using TeamsCore.Models.Responses;

namespace TeamsCore.Implementations.Queries.Handlers
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, QueryRecordResponse<ProductResponse>>
    {
        private readonly ILogger<GetProductQueryHandler> _logger;
        private readonly GetProductQueryValidator _validator;
        private readonly IProductRepository _productRepository;
        private readonly IAuthenticationContextProvider _authenticationContext;

        public GetProductQueryHandler(
            ILogger<GetProductQueryHandler> logger,
            GetProductQueryValidator validator,
            IProductRepository productRepository,
            IAuthenticationContextProvider authenticationContext)
        {
            _logger = logger;
            _validator = validator;
            _productRepository = productRepository;
            _authenticationContext = authenticationContext;
        }


        public async Task<QueryRecordResponse<ProductResponse>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _productRepository.GetProduct(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordResponse<ProductResponse>().BuildErrorResponse(
                    Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContext.GetAuthenticationContext().RequestUri));
            }
        }
    }
}
