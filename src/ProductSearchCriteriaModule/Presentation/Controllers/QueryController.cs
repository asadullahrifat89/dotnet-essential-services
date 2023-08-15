using BaseModule.Application.DTOs.Responses;
using CommonModule.Infrastructure.Constants;
using IdentityModule.Infrastructure.Attributes;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductSearchCriteriaModule.Application.Queries;
using ProductSearchCriteriaModule.Domain.Entities;

namespace ProductSearchCriteriaModule.Presentation.Controllers
{
    [ApiController]
    [AuthorizationRequired]
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

        [HttpGet(EndpointRoutes.Action_GetProductSearchCriteria)]
        public async Task<QueryRecordResponse<ProductSearchCriteria>> GetProductSearchCriteria([FromQuery] GetProductSearchCriteriaQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet(EndpointRoutes.Action_GetProductSearchCriterias)]
        public async Task<QueryRecordsResponse<ProductSearchCriteria>> GetProductSearchCriterias([FromQuery] GetProductSearchCriteriasQuery query)
        {
            return await _mediator.Send(query);
        }

        #endregion
    }
}
