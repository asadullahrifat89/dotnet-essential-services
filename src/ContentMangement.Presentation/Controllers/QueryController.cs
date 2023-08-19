using Base.Application.Attributes;
using Base.Application.DTOs.Responses;
using Base.Shared.Constants;
using Identity.Application.Attributes;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Teams.ContentMangement.Application.Queries;
using Teams.ContentMangement.Domain.Entities;

namespace Teams.ContentMangement.Presentation.Controllers
{
    [ApiController]
    [AuthorizationRequired]
    [ControllerName("Teams.ContentMangement.Query")]
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

        [HttpGet(EndpointRoutes.Action_GetProductSearchCriteriasForProductIdQuery)]
        public async Task<QueryRecordsResponse<ProductSearchCriteria>> GetProductSearchCriteriasForProductIdQuery([FromQuery] GetProductSearchCriteriasForProductIdQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet(EndpointRoutes.Action_GetProduct)]
        public async Task<QueryRecordResponse<Product>> GetProduct([FromQuery] GetProductQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet(EndpointRoutes.Action_GetProducts)]
        public async Task<QueryRecordsResponse<Product>> GetProducts([FromQuery] GetProductsQuery query)
        {
            return await _mediator.Send(query);
        }

       

        [HttpGet(EndpointRoutes.Action_GetProject)]
        public async Task<QueryRecordResponse<Project>> GetProject([FromQuery] GetProjectQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet(EndpointRoutes.Action_GetProjects)]
        public async Task<QueryRecordsResponse<Project>> GetProjects([FromQuery] GetProjectsQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet(EndpointRoutes.Action_GetProjectsForProductId)]
        public async Task<QueryRecordsResponse<Project>> GetProjectsForProductId([FromQuery] GetProjectsForProductIdQuery query)
        {
            return await _mediator.Send(query);
        }

        #endregion
    }
}
