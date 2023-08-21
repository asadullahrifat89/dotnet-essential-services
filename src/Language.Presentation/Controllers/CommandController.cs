using Base.Application.Attributes;
using Base.Application.DTOs.Responses;
using Base.Shared.Constants;
using Language.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Language.Presentation.Controllers
{
    [ApiController]
    [AuthorizationRequired]
    [ControllerName("Language.Command")]
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

        [HttpPost(EndpointRoutes.Action_AddLanguageApp)]
        public async Task<ServiceResponse> AddLingoApp(AddLingoAppCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPost(EndpointRoutes.Action_AddLanguageResources)]
        public async Task<ServiceResponse> AddLingoResources(AddLingoResourcesCommand command)
        {
            return await _mediator.Send(command);
        }

        #endregion
    }
}
