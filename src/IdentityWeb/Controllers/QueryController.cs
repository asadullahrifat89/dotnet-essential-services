using IdentityCore.Contracts.Declarations.Queries;
using IdentityCore.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using IdentityCore.Attributes;

namespace IdentityWeb.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]

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

        [HttpGet("GetUser")]
        public async Task<QueryRecordResponse<UserResponse>> GetUser([FromQuery] GetUserQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet("GetUsers")]
        public async Task<QueryRecordsResponse<UserResponse>> GetUsers([FromQuery]GetUsersQuery query)
        {
            return await _mediator.Send(query);
        }

        #endregion
    }
}