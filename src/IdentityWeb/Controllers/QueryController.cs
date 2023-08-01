using IdentityCore.Contracts.Declarations.Queries;
using IdentityCore.Models.Responses;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace IdentityWeb.Controllers
{
    [ApiController]
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

        [HttpGet]
        public async Task<QueryRecordResponse<UserResponse>> GetUser(string id)
        {
            return await _mediator.Send(new GetUserQuery() { UserId = id });
        }

        #endregion
    }
}