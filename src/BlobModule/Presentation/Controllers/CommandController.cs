using BaseModule.Application.DTOs.Responses;
using BaseModule.Infrastructure.Attributes;
using BlobModule.Application.Commands;
using CommonModule;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BlobModule.Presentation.Controllers
{
    [ApiController]
    [AuthorizationRequired]
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
