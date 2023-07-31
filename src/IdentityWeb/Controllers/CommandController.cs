using IdentityCore.Contracts.Declarations.Commands;
using IdentityCore.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityWeb.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CommandController : ControllerBase
    {
        #region Fields

        private readonly ILogger<CommandController> _logger;
        private readonly IMediator _mediator;
        private readonly IHttpContextAccessor _httpContextAccessor;

        #endregion

        #region Ctor

        public CommandController(ILogger<CommandController> logger, IMediator mediator, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _mediator = mediator;
            _httpContextAccessor = httpContextAccessor;
        }

        #endregion

        #region Methods
        
        [HttpPost("CreateUser")]        
        public async Task<ServiceResponse> CreateUser(CreateUserCommand command)
        {
            return await _mediator.Send(command);
        }

        [HttpPost("Authenticate")]
        [AllowAnonymous]
        public async Task<ServiceResponse> Authenticate()
        {
            var httpContext = _httpContextAccessor.HttpContext;

            if (httpContext is null)
                return new ServiceResponse().BuildBadRequestResponse("Bad request");

            var command = new AuthenticateCommand()
            {
                Email = httpContext.Request.Form["Email"].ToString(),
                Password = httpContext.Request.Form["Password"].ToString(),
            };

            return await _mediator.Send(command);
        }

        [HttpPost("Validate")]
        [AllowAnonymous]
        public async Task<ServiceResponse> Validate(ValidateTokenCommand command)
        {
            return await _mediator.Send(command);
        }

        #endregion
    }
}