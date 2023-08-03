using FluentValidation;
using IdentityCore.Contracts.Declarations.Queries;
using IdentityCore.Contracts.Declarations.Repositories;
using IdentityCore.Contracts.Declarations.Services;
using IdentityCore.Contracts.Implementations.Queries.Validators;
using IdentityCore.Extensions;
using IdentityCore.Models.Entities;
using IdentityCore.Models.Responses;
using MediatR;
using Microsoft.Extensions.Logging;

namespace IdentityCore.Contracts.Implementations.Queries.Handlers
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

