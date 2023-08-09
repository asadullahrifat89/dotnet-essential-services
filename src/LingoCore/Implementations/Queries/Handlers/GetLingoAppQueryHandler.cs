using BaseCore.Extensions;
using BaseCore.Models.Responses;
using BaseCore.Services;
using LingoCore.Declarations.Queries;
using LingoCore.Declarations.Repositories;
using LingoCore.Implementations.Queries.Validators;
using LingoCore.Models.Entities;
using MediatR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LingoCore.Implementations.Queries.Handlers
{
    public class GetLingoAppQueryHandler : IRequestHandler<GetLingoAppQuery, QueryRecordResponse<LingoApp>>
    {
        private readonly ILogger<GetLingoAppQueryHandler> _logger;
        private readonly GetLingoAppQueryValidator _validator;
        private readonly ILingoAppRepository _lingoAppRepository;
        private readonly IAuthenticationContextProvider _authenticationContext;

        public GetLingoAppQueryHandler (
            ILogger<GetLingoAppQueryHandler> logger, GetLingoAppQueryValidator validator,
            ILingoAppRepository lingoAppRepository, IAuthenticationContextProvider authenticationContext)
        {
            _logger = logger;
            _validator = validator;
            _lingoAppRepository = lingoAppRepository;
            _authenticationContext = authenticationContext;
        }

        public async Task<QueryRecordResponse<LingoApp>> Handle(GetLingoAppQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _lingoAppRepository.GetLingoApp(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordResponse<LingoApp>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContext.GetAuthenticationContext().RequestUri));
        
            }
        }
    }
}
