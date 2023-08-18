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
    public class GetProductSearchCriteriaQueryHandler : IRequestHandler<GetProductSearchCriteriaQuery, QueryRecordResponse<ProductSearchCriteria>>
    {
        #region Fields

        private readonly ILogger<GetProductSearchCriteriaQueryHandler> _logger;
        private readonly GetProductSearchCriteriaQueryValidator _validator;
        private readonly IProductSearchCriteriaRepository _productSearchCriteriaRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctors

        public GetProductSearchCriteriaQueryHandler(
            ILogger<GetProductSearchCriteriaQueryHandler> logger,
            GetProductSearchCriteriaQueryValidator validator,
            IProductSearchCriteriaRepository ProductSearchCriteriaRepository,
            IAuthenticationContextProvider authenticationContextProvider)
        {
            _logger = logger;
            _validator = validator;
            _productSearchCriteriaRepository = ProductSearchCriteriaRepository;
            _authenticationContextProvider = authenticationContextProvider;
        }

        #endregion

        public async Task<QueryRecordResponse<ProductSearchCriteria>> Handle(GetProductSearchCriteriaQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();
                var result = await _productSearchCriteriaRepository.GetProductSearchCriteria(request.ProductSearchCriteriaId);

                return Response.BuildQueryRecordResponse<ProductSearchCriteria>().BuildSuccessResponse(result, authCtx?.RequestUri);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordResponse<ProductSearchCriteria>().BuildErrorResponse(
                    Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContextProvider.GetAuthenticationContext().RequestUri));
            }
        }
    }
}
