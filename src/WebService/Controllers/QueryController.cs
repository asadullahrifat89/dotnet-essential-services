using MediatR;
using Microsoft.AspNetCore.Mvc;
using LanguageModule.Declarations.Queries;
using LanguageModule.Models.Entities;
using BaseModule.Application.DTOs.Responses;
using CommonModule.Infrastructure.Constants;
using IdentityModule.Application.Queries;
using IdentityModule.Domain.Entities;
using IdentityModule.Application.DTOs;
using IdentityModule.Infrastructure.Attributes;

namespace WebService.Controllers
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

        #region Blob

        //[HttpGet(EndpointRoutes.Action_DownloadFile)]
        //public async Task<IActionResult> DownloadFile([FromQuery] DownloadBlobFileQuery query)
        //{
        //    var blobFileResponse = await _mediator.Send(query);

        //    return File(blobFileResponse.Bytes, blobFileResponse.ContentType);
        //}

        //[HttpGet(EndpointRoutes.Action_GetFile)]
        //public async Task<QueryRecordResponse<BlobFile>> GetBlobFiles([FromQuery] GetBlobFileQuery query)
        //{
        //    return await _mediator.Send(query);
        //}

        #endregion

        #region EmailTemplate

        //[HttpGet(EndpointRoutes.Action_GetEmailTemplate)]
        //public async Task<QueryRecordResponse<EmailTemplate>> GetEmailTemplate([FromQuery] GetEmailTemplateQuery query)
        //{
        //    return await _mediator.Send(query);
        //}

        #endregion

        #region Expired

        //[HttpGet(EndpointRoutes.Action_GetEndPoints)]
        //public async Task<QueryRecordsResponse<string>> GetEndPoints([FromQuery] GetEndPointsQuery query)
        //{
        //    return await _mediator.Send(query);
        //} 

        #endregion

        #region LingoResources

        [HttpGet(EndpointRoutes.Action_GetLingoResourcesInFormat)]

        public async Task<QueryRecordResponse<Dictionary<string, string>>> GetLingoResourcesInFormat([FromQuery] GetLingoResourcesInFormatQuery query)
        {
            return await _mediator.Send(query);
        }

        [HttpGet(EndpointRoutes.Action_GetLingoApp)]

        public async Task<QueryRecordResponse<LingoApp>> GetLingoApp([FromQuery] GetLingoAppQuery query)
        {
            return await _mediator.Send(query);
        }

        #endregion

        #endregion
    }
}