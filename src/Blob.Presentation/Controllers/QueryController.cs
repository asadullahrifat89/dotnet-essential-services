using Base.Application.Attributes;
using Base.Application.DTOs.Responses;
using Base.Shared.Constants;
using Blob.Application.Queries;
using Blob.Domain.Entities;
using Identity.Application.Attributes;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blob.Presentation.Controllers
{
    [ApiController]
    [AuthorizationRequired]
    [ControllerName("Blob.Query")]
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

        [HttpGet(EndpointRoutes.Action_DownloadFile)]
        public async Task<IActionResult> DownloadFile([FromQuery] DownloadBlobFileQuery query)
        {
            var blobFileResponse = await _mediator.Send(query);

            return File(blobFileResponse.Bytes, blobFileResponse.ContentType);
        }

        [HttpGet(EndpointRoutes.Action_GetFile)]
        public async Task<QueryRecordResponse<BlobFile>> GetBlobFiles([FromQuery] GetBlobFileQuery query)
        {
            return await _mediator.Send(query);
        }

        #endregion
    }
}
