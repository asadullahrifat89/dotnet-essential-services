using BaseModule.Application.DTOs.Responses;
using BaseModule.Infrastructure.Extensions;
using IdentityModule.Application.Providers.Interfaces;
using LanguageModule.Application.Queries.Validators;
using LanguageModule.Domain.Entities;
using LanguageModule.Domain.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LanguageModule.Application.Queries.Handlers
{
    public class GetLingoAppQueryHandler : IRequestHandler<GetLingoAppQuery, QueryRecordResponse<LanguageApp>>
    {
        private readonly ILogger<GetLingoAppQueryHandler> _logger;
        private readonly GetLingoAppQueryValidator _validator;
        private readonly ILingoAppRepository _lingoAppRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        public GetLingoAppQueryHandler(
            ILogger<GetLingoAppQueryHandler> logger, GetLingoAppQueryValidator validator,
            ILingoAppRepository lingoAppRepository, IAuthenticationContextProvider authenticationContext)
        {
            _logger = logger;
            _validator = validator;
            _lingoAppRepository = lingoAppRepository;
            _authenticationContextProvider = authenticationContext;
        }

        public async Task<QueryRecordResponse<LanguageApp>> Handle(GetLingoAppQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _lingoAppRepository.GetLingoApp(request.AppId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordResponse<LanguageApp>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContextProvider.GetAuthenticationContext().RequestUri));

            }
        }
    }
}
