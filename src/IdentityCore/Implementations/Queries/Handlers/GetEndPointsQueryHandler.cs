using IdentityCore.Declarations.Queries;
using IdentityCore.Declarations.Repositories;
using IdentityCore.Declarations.Services;
using IdentityCore.Models.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityCore.Implementations.Queries.Handlers
{
    public class GetEndPointsQueryHandler : IRequestHandler<GetEndPointsQuery, QueryRecordsResponse<string>>
    {
        private readonly ILogger<GetUserQueryHandler> _logger;
        private readonly IEndpointsRepository _endpointsRepository;
        private readonly IAuthenticationContextProvider _authenticationContext;


        public GetEndPointsQueryHandler(
            ILogger<GetUserQueryHandler> logger,
            IEndpointsRepository endpointsRepository,
            IAuthenticationContextProvider authenticationContext)
        {
            _logger = logger;
            _endpointsRepository = endpointsRepository;
            _authenticationContext = authenticationContext;
        }

        public async Task<QueryRecordsResponse<string>> Handle(GetEndPointsQuery request, CancellationToken cancellationToken)
        {
            try
            {
                return await _endpointsRepository.GetEndpointList(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occured while getting endpoints");
                return Response.BuildQueryRecordsResponse<string>().BuildErrorResponse(
                    Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContext.GetAuthenticationContext().RequestUri));
            }
        }
    }

}

