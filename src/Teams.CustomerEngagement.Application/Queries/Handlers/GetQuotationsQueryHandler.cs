using Base.Application.DTOs.Responses;
using Identity.Application.Providers.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Teams.CustomerEngagement.Application.Queries.Validators;
using Teams.CustomerEngagement.Domain.Repositories.Interfaces;
using Teams.CustomerEngagement.Domain.Entities;
using Base.Application.Extensions;

namespace Teams.CustomerEngagement.Application.Queries.Handlers
{
    public class GetQuotationsQueryHandler : IRequestHandler<GetQuotationsQuery, QueryRecordsResponse<Quotation>>
    {

        #region Fields

        private readonly ILogger<GetQuotationsQueryHandler> _logger;
        private readonly GetQuotationsQueryValidator _validator;
        private readonly IQuotationRepository _QuotationRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctors

        public GetQuotationsQueryHandler(
            ILogger<GetQuotationsQueryHandler> logger,
            GetQuotationsQueryValidator validator,
            IAuthenticationContextProvider authenticationContextProvider,
            IQuotationRepository QuotationRepository)
        {
            _logger = logger;
            _validator = validator;
            _authenticationContextProvider = authenticationContextProvider;
            _QuotationRepository = QuotationRepository;
        }

        #endregion

        #region Methods

        public async Task<QueryRecordsResponse<Quotation>> Handle(GetQuotationsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();
                var result = await _QuotationRepository.GetQuotations(
                    searchTerm: request.SearchTerm,
                    pageIndex: request.PageIndex,
                    pageSize: request.PageSize,
                    priority: request.Priority,
                    fromDate: request.FromDate,
                    toDate: request.ToDate,
                    location: request.Location);

                return Response.BuildQueryRecordsResponse<Quotation>().BuildSuccessResponse(count: result.Count, records: result.Records, authCtx?.RequestUri);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordsResponse<Quotation>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContextProvider.GetAuthenticationContext().RequestUri));
            }
        }

        #endregion
    }
}
