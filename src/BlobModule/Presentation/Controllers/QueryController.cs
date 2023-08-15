using BaseModule.Application.DTOs.Responses;
using BlobModule.Application.Queries;
using BlobModule.Domain.Entities;
using CommonModule;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlobModule.Presentation.Controllers
{
    [ApiController]
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
