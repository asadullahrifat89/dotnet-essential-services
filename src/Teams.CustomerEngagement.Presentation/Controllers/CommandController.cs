using Base.Application.Attributes;
using Base.Application.DTOs.Responses;
using Base.Shared.Constants;
using Identity.Application.Attributes;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Teams.CustomerEngagement.Application.Commands;

namespace Teams.CustomerEngagement.Presentation.Controllers
{
    [ApiController]
    [AuthorizationRequired]
    [ControllerName("Teams.CustomerEngagement.Command")]
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

        [AuthorizationNotRequired]
        [HttpPost(EndpointRoutes.Action_AddQuotation)]
        public async Task<ServiceResponse> AddAddQuotation(AddQuotationCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPut(EndpointRoutes.Action_UpdateQuotation)]
        public async Task<ServiceResponse> AddUpdateQuotation(UpdateQuotationCommand command)
        {
            return await _mediator.Send(command);
        }

        #endregion
    }
}
