using Base.Application.Attributes;
using Base.Application.DTOs.Responses;
using Base.Shared.Constants;
using Email.Application.Commands;
using Identity.Application.Attributes;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Email.Presentation.Controllers
{
    [ApiController]
    [AuthorizationRequired]
    [ControllerName("Email.Command")]
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

        [HttpPost(EndpointRoutes.Action_CreateEmailTemplate)]
        public async Task<ServiceResponse> CreateEmailTemplate(CreateEmailTemplateCommand command)
        {
            return await _mediator.Send(command);
        }


        [HttpPut(EndpointRoutes.Action_UpdateEmailTemplate)]
        public async Task<ServiceResponse> UpdateEmailTemplate(UpdateEmailTemplateCommand command)
        {
            return await _mediator.Send(command);
        }


        [HttpPost(EndpointRoutes.Action_EnqueueEmailMessage)]
        public async Task<ServiceResponse> EnqueueEmailMessage(EnqueueEmailMessageCommand command)
        {
            return await _mediator.Send(command);
        }

        #endregion
    }
}
