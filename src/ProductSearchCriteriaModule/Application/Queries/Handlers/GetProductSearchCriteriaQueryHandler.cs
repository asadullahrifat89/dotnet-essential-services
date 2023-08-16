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
    public class GetProductSearchCriteriaQueryHandler : IRequestHandler<GetProductSearchCriteriaQuery, QueryRecordResponse<ProductSearchCriteria>>
    {
        #region Fields

        private readonly ILogger<GetProductSearchCriteriaQueryHandler> _logger;
        private readonly GetProductSearchCriteriaQueryValidator _validator;
        private readonly IProductSearchCriteriaRepository _ProductSearchCriteriaRepository;
        private readonly IAuthenticationContextProvider _authenticationContext;

        #endregion

        #region Ctors

        public GetProductSearchCriteriaQueryHandler(
            ILogger<GetProductSearchCriteriaQueryHandler> logger,
            GetProductSearchCriteriaQueryValidator validator,
            IProductSearchCriteriaRepository ProductSearchCriteriaRepository,
            IAuthenticationContextProvider authenticationContext)
        {
            _logger = logger;
            _validator = validator;
            _ProductSearchCriteriaRepository = ProductSearchCriteriaRepository;
            _authenticationContext = authenticationContext;
        }

        #endregion

        public async Task<QueryRecordResponse<ProductSearchCriteria>> Handle(GetProductSearchCriteriaQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _ProductSearchCriteriaRepository.GetProductSearchCriteria(request.ProductSearchCriteriaId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordResponse<ProductSearchCriteria>().BuildErrorResponse(
                    Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContext.GetAuthenticationContext().RequestUri));
            }
        }
    }
}
