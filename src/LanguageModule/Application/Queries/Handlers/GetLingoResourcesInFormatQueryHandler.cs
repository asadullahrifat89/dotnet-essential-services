using BaseModule.Application.DTOs.Responses;
using BaseModule.Infrastructure.Extensions;
using IdentityModule.Application.Providers.Interfaces;
using LanguageModule.Application.Queries.Validators;
using LanguageModule.Domain.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LanguageModule.Application.Queries.Handlers
{
    public class GetLingoResourcesInFormatQueryHandler : IRequestHandler<GetLingoResourcesInFormatQuery, QueryRecordResponse<Dictionary<string, string>>>
    {
        #region Fields

        private readonly ILogger<GetLingoResourcesInFormatQueryHandler> _logger;
        private readonly GetLingoResourcesInFormatQueryValidator _validator;
        private readonly ILingoResourcesRepository _lingoResourceRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public GetLingoResourcesInFormatQueryHandler(
            ILogger<GetLingoResourcesInFormatQueryHandler> logger,
            GetLingoResourcesInFormatQueryValidator validator,
            ILingoResourcesRepository lingoAppRepository,
            IAuthenticationContextProvider authenticationContext)
        {
            _logger = logger;
            _validator = validator;
            _lingoResourceRepository = lingoAppRepository;
            _authenticationContextProvider = authenticationContext;
        }

        #endregion

        #region Methods
        public async Task<QueryRecordResponse<Dictionary<string, string>>> Handle(GetLingoResourcesInFormatQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _lingoResourceRepository.GetLingoResourcesInFormat(
                    appId: request.AppId,
                    format: request.Format,
                    languageCode: request.LanguageCode);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordResponse<Dictionary<string, string>>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContextProvider.GetAuthenticationContext().RequestUri));
            }
        }

        #endregion
    }
}
