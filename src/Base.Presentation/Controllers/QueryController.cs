using Base.Application.Attributes;
using Base.Application.DTOs.Responses;
using Base.Shared.Constants;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Base.Presentation.Controllers
{
    [ApiController]
    [AuthorizationNotRequired]
    [ControllerName("Base.Query")]
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

        [HttpGet(EndpointRoutes.Action_Ping)]
        public async Task<string> Ping()
        {
            return "I am alive.";
        }
    }
}
