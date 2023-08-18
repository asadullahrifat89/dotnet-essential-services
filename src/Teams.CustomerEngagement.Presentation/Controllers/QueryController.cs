using Base.Application.Attributes;
using Base.Application.DTOs.Responses;
using Base.Shared.Constants;
using Identity.Application.Attributes;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Teams.ContentMangement.Application.DTOs.Responses;
using Teams.CustomerEngagement.Application.Queries;
using Teams.CustomerEngagement.Domain.Entities;

namespace Teams.CustomerEngagement.Presentation.Controllers
{
    [ApiController]
    [AuthorizationRequired]
    [ControllerName("Teams.CustomerEngagement.Query")]
    public class QueryController : ControllerBase
    {
        #region Fields

        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Ctor

        public QueryController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Methods

        [HttpGet(EndpointRoutes.Action_GetQuotation)]
        public async Task<QueryRecordResponse<Quotation>> GetQuotation([FromQuery] GetQuotationQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet(EndpointRoutes.Action_GetQuotations)]
        public async Task<QueryRecordsResponse<Quotation>> GetQuotations([FromQuery] GetQuotationsQuery query)
        {
            return await _mediator.Send(query);
        }

        [AuthorizationNotRequired]
        [HttpPost(EndpointRoutes.Action_GetProductRecommendations)]
        public async Task<QueryRecordsResponse<ProductRecommendationResponse>> GetProductRecommendations([FromBody] GetProductRecommendationsQuery query)
        {
            return await _mediator.Send(query);
        }

        #endregion
    }
}
