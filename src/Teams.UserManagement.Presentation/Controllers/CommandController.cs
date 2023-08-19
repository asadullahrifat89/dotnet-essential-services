using Base.Application.Attributes;
using Base.Application.DTOs.Responses;
using Base.Shared.Constants;
using Identity.Application.Attributes;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Teams.UserManagement.Application.Commands;

namespace Teams.UserManagement.Presentation.Controllers
{
    [ApiController]
    [AuthorizationRequired]
    [ControllerName("Teams.UserManagement.Command")]
    public class CommandController
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
        [HttpPost(EndpointRoutes.Action_OnboardUser)]
        public async Task<ServiceResponse> OnboardUser(OnboardUserCommand command)
        {
            return await _mediator.Send(command);
        }       

        #endregion
    }
}
