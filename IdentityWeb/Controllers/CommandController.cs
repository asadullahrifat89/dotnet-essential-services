using IdentityCore.Contracts.Declarations.Commands;
using IdentityCore.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Authorization;
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

        #endregion

        #region Ctor

        public CommandController(ILogger<CommandController> logger, IMediator mediator)
        {
            _logger = logger;
            _mediator = mediator;
        }

        #endregion

        #region Methods
        
        [HttpPost("Signup")]
        [AllowAnonymous]
        public async Task<ServiceResponse> Signup(SignupCommand command)
        {
            return await _mediator.Send(command);
        }

        #endregion
    }
}