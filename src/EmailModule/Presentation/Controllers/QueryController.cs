using BaseModule.Application.DTOs.Responses;
using CommonModule.Infrastructure.Constants;
using EmailModule.Application.Queries;
using EmailModule.Domain.Entities;
using IdentityModule.Infrastructure.Attributes;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace EmailModule.Presentation.Controllers
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

        [HttpGet(EndpointRoutes.Action_GetEmailTemplate)]
        public async Task<QueryRecordResponse<EmailTemplate>> GetEmailTemplate([FromQuery] GetEmailTemplateQuery query)
        {
            return await _mediator.Send(query);
        }

        #endregion
    }
}
