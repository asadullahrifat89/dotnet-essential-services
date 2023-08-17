using Base.Application.DTOs.Responses;
using Base.Shared.Constants;
using Identity.Application.Attributes;
using Language.Application.Queries;
using Language.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Language.Presentation.Controllers
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

        [HttpGet(EndpointRoutes.Action_GetLingoResourcesInFormat)]

        public async Task<QueryRecordResponse<Dictionary<string, string>>> GetLingoResourcesInFormat([FromQuery] GetLingoResourcesInFormatQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet(EndpointRoutes.Action_GetLingoApp)]

        public async Task<QueryRecordResponse<LanguageApp>> GetLingoApp([FromQuery] GetLingoAppQuery query)
        {
            return await _mediator.Send(query);
        }

        #endregion
    }
}
