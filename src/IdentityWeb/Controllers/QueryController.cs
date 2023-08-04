using IdentityCore.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using IdentityCore.Attributes;
using IdentityCore;
using IdentityCore.Models.Entities;
using IdentityCore.Declarations.Queries;

namespace IdentityWeb.Controllers
{
    [ApiController]
    [AuthorizationRequired]
    //[AuthorizationNotRequired]
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

        [HttpGet(EndpointRoutes.Action_GetUser)]
        public async Task<QueryRecordResponse<UserResponse>> GetUser([FromQuery] GetUserQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet(EndpointRoutes.Action_GetUsers)]
        public async Task<QueryRecordsResponse<UserResponse>> GetUsers([FromQuery] GetUsersQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet(EndpointRoutes.Action_GetEndPoints)]
        public async Task<QueryRecordsResponse<string>> GetEndPoints([FromQuery] GetEndPointsQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet(EndpointRoutes.Action_GetRoles)]
        public async Task<QueryRecordsResponse<Role>> GetRoles([FromQuery] GetRolesQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet(EndpointRoutes.Action_GetClaims)]
        public async Task<QueryRecordsResponse<ClaimPermission>> GetClaims([FromQuery] GetClaimsQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet(EndpointRoutes.Action_GetUserRoles)]
        public async Task<QueryRecordsResponse<Role>> GetUserRoles([FromQuery] GetUserRolesQuery query)
        {
            return await _mediator.Send(query);
        }

        #endregion
    }
}