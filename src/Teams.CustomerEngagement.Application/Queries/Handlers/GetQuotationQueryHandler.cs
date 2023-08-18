using Base.Application.DTOs.Responses;
using Identity.Application.Providers.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teams.ContentMangement.Application.Queries.Validators;
using Teams.ContentMangement.Application.Queries;
using Teams.ContentMangement.Domain.Entities;
using Teams.ContentMangement.Domain.Repositories.Interfaces;
using Teams.CustomerEngagement.Application.Queries.Validators;
using Teams.CustomerEngagement.Domain.Repositories.Interfaces;
using Teams.CustomerEngagement.Domain.Entities;
using Base.Application.Extensions;

namespace Teams.CustomerEngagement.Application.Queries.Handlers
{
    public class GetQuotationQueryHandler : IRequestHandler<GetQuotationQuery, QueryRecordResponse<Quotation>>
    {
        private readonly ILogger<GetQuotationQueryHandler> _logger;
        private readonly GetQuotationQueryValidator _validator;
        private readonly IQuotationRepository _QuotationRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        public GetQuotationQueryHandler(
            ILogger<GetQuotationQueryHandler> logger,
            GetQuotationQueryValidator validator,
            IQuotationRepository QuotationRepository,
            IAuthenticationContextProvider authenticationContextProvider)
        {
            _logger = logger;
            _validator = validator;
            _QuotationRepository = QuotationRepository;
            _authenticationContextProvider = authenticationContextProvider;
        }


        public async Task<QueryRecordResponse<Quotation>> Handle(GetQuotationQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();
                var result = await _QuotationRepository.GetQuotation(request.QuotationId);

                return Response.BuildQueryRecordResponse<Quotation>().BuildSuccessResponse(result, authCtx?.RequestUri);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordResponse<Quotation>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContextProvider.GetAuthenticationContext().RequestUri));
            }
        }
    }
}
