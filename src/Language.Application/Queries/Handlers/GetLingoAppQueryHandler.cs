using Base.Application.DTOs.Responses;
using Base.Application.Extensions;
using Identity.Application.Providers.Interfaces;
using Language.Application.Queries.Validators;
using Language.Domain.Entities;
using Language.Domain.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Language.Application.Queries.Handlers
{
    public class GetLingoAppQueryHandler : IRequestHandler<GetLingoAppQuery, QueryRecordResponse<LanguageApp>>
    {
        private readonly ILogger<GetLingoAppQueryHandler> _logger;
        private readonly GetLingoAppQueryValidator _validator;
        private readonly ILanguageAppRepository _lingoAppRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        public GetLingoAppQueryHandler(
            ILogger<GetLingoAppQueryHandler> logger, GetLingoAppQueryValidator validator,
            ILanguageAppRepository lingoAppRepository, IAuthenticationContextProvider authenticationContext)
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

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();

                var result = await _lingoAppRepository.GetLingoApp(request.AppId);

                return Response.BuildQueryRecordResponse<LanguageApp>().BuildSuccessResponse(result, authCtx?.RequestUri);

            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordResponse<LanguageApp>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContextProvider.GetAuthenticationContext().RequestUri));

            }
        }
    }
}
