using BaseModule.Application.DTOs.Responses;
using BaseModule.Infrastructure.Extensions;
using IdentityModule.Infrastructure.Services.Interfaces;
using LanguageModule.Declarations.Queries;
using LanguageModule.Declarations.Repositories;
using LanguageModule.Implementations.Queries.Validators;
using LanguageModule.Models.Entities;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LanguageModule.Implementations.Queries.Handlers
{
    public class GetLingoAppQueryHandler : IRequestHandler<GetLingoAppQuery, QueryRecordResponse<LingoApp>>
    {
        private readonly ILogger<GetLingoAppQueryHandler> _logger;
        private readonly GetLingoAppQueryValidator _validator;
        private readonly ILingoAppRepository _lingoAppRepository;
        private readonly IAuthenticationContextProviderService _authenticationContext;

        public GetLingoAppQueryHandler(
            ILogger<GetLingoAppQueryHandler> logger, GetLingoAppQueryValidator validator,
            ILingoAppRepository lingoAppRepository, IAuthenticationContextProviderService authenticationContext)
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
