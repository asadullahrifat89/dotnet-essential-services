using Base.Application.DTOs.Responses;
using Base.Shared.Constants;
using Identity.Application.Attributes;
using Identity.Application.DTOs;
using Identity.Application.Queries;
using Identity.Domain.Entities;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IdentityModule.Presentation.Controllers
{
    [ApiController]
    [AuthorizationRequired]
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

        #region User

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

        #endregion

        #region Role

        [HttpGet(EndpointRoutes.Action_GetRoles)]
        public async Task<QueryRecordsResponse<Role>> GetRoles([FromQuery] GetRolesQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet(EndpointRoutes.Action_GetUserRoles)]
        public async Task<QueryRecordsResponse<Role>> GetUserRoles([FromQuery] GetUserRolesQuery query)
        {
            return await _mediator.Send(query);
        }

        #endregion

        #region Claim

        [HttpGet(EndpointRoutes.Action_GetClaims)]
        public async Task<QueryRecordsResponse<ClaimPermission>> GetClaims([FromQuery] GetClaimsQuery query)
        {
            return await _mediator.Send(query);
        }

        #endregion

        #endregion
    }
}
