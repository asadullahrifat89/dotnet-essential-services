using Base.Application.DTOs.Responses;
using Base.Application.Extensions;
using FluentValidation;
using Identity.Application.Providers.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teams.ContentMangement.Application.Queries.Validators;
using Teams.ContentMangement.Domain.Entities;
using Teams.ContentMangement.Domain.Repositories.Interfaces;

namespace Teams.ContentMangement.Application.Queries.Handlers
{
    public class GetProductSearchCriteriasForProductIdQueryHandler : IRequestHandler<GetProductSearchCriteriasForProductIdQuery, QueryRecordsResponse<ProductSearchCriteria>>
    {
        #region Fields

        private readonly ILogger<GetProductSearchCriteriasForProductIdQueryHandler> _logger;
        private readonly GetProductSearchCriteriasForProductIdQueryValidator _validator;
        private readonly IProductSearchCriteriaRepository _productSearchCriteriaRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public GetProductSearchCriteriasForProductIdQueryHandler(
          ILogger<GetProductSearchCriteriasForProductIdQueryHandler> logger,
          GetProductSearchCriteriasForProductIdQueryValidator validator,
          IProductSearchCriteriaRepository productSearchCriteriaRepository,
          IAuthenticationContextProvider authenticationContextProvider)
        {
            _logger = logger;
            _validator = validator;
            _productSearchCriteriaRepository = productSearchCriteriaRepository;
            _authenticationContextProvider = authenticationContextProvider;
        }

        #endregion

        public async Task<QueryRecordsResponse<ProductSearchCriteria>> Handle(GetProductSearchCriteriasForProductIdQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();
                var result = await _productSearchCriteriaRepository.GetProductSearchCriteriasForProductId(request.ProductId);

                return Response.BuildQueryRecordsResponse<ProductSearchCriteria>().BuildSuccessResponse(result.Count, result.Records, authCtx?.RequestUri);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordsResponse<ProductSearchCriteria>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContextProvider.GetAuthenticationContext().RequestUri));
            }
        }
    }
}
