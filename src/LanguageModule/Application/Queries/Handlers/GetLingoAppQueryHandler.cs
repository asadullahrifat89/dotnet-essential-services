using BaseModule.Application.DTOs.Responses;
using BaseModule.Infrastructure.Extensions;
using IdentityModule.Infrastructure.Services.Interfaces;
using LanguageModule.Application.Queries;
using LanguageModule.Application.Queries.Validators;
using LanguageModule.Domain.Entities;
using LanguageModule.Domain.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LanguageModule.Application.Queries.Handlers
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
