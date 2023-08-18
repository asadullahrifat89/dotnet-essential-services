using Base.Application.Attributes;
using Base.Application.DTOs.Responses;
using Base.Shared.Constants;
using Blob.Application.Commands;
using Identity.Application.Attributes;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Blob.Presentation.Controllers
{
    [ApiController]
    [AuthorizationRequired]
    [ControllerName("Blob.Commands")]
    public class CommandController : ControllerBase
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

        [HttpPost(EndpointRoutes.Action_UploadFile)]        
        public async Task<ServiceResponse> UploadFile(IFormFile file)
        {
            return await _mediator.Send(new UploadBlobFileCommand() { FormFile = file });
        }

        #endregion
    }
}
