using BaseModule.Application.DTOs.Responses;
using CommonModule.Infrastructure.Constants;
using IdentityModule.Infrastructure.Attributes;
using LanguageModule.Application.Commands;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LanguageModule.Presentation.Controllers
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

        [HttpPost(EndpointRoutes.Action_AddLingoApp)]
        public async Task<ServiceResponse> AddLingoApp(AddLingoAppCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPost(EndpointRoutes.Action_AddLingoResources)]
        public async Task<ServiceResponse> AddLingoResources(AddLingoResourcesCommand command)
        {
            return await _mediator.Send(command);
        }

        #endregion
    }
}
