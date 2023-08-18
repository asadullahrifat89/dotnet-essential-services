﻿using Base.Application.DTOs.Responses;
using Base.Shared.Constants;
using ContentMangement.Application.Queries;
using ContentMangement.Domain.Entities;
using Identity.Application.Attributes;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ContentMangement.Presentation.Controllers
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