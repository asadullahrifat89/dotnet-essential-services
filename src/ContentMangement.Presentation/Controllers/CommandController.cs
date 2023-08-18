using Base.Application.DTOs.Responses;
using Base.Shared.Constants;
using Identity.Application.Attributes;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Teams.ContentMangement.Application.Commands;

namespace Teams.ContentMangement.Presentation.Controllers
{
    [ApiController]
    [AuthorizationRequired]
    public class CommandController : ControllerBase
    {
        #region Fields

        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Ctor

        public CommandController(IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Methods

        [HttpPost(EndpointRoutes.Action_AddProductSearchCriteria)]
        public async Task<ServiceResponse> AddProductSearchCriteria(AddProductSearchCriteriaCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut(EndpointRoutes.Action_UpdateProductSearchCriteria)]
        public async Task<ServiceResponse> UpdateProductSearchCriteria(UpdateProductSearchCriteriaCommand command)
        {
            return await _mediator.Send(command);
        }

        #endregion
    }
}
