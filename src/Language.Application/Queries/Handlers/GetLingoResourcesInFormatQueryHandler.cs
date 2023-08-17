using Base.Application.DTOs.Responses;
using Base.Application.Extensions;
using Identity.Application.Providers.Interfaces;
using Language.Application.Queries;
using Language.Application.Queries.Validators;
using Language.Domain.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Language.Application.Queries.Handlers
{
    public class GetLingoResourcesInFormatQueryHandler : IRequestHandler<GetLingoResourcesInFormatQuery, QueryRecordResponse<Dictionary<string, string>>>
    {
        #region Fields

        private readonly ILogger<GetLingoResourcesInFormatQueryHandler> _logger;
        private readonly GetLingoResourcesInFormatQueryValidator _validator;
        private readonly ILanguageResourcesRepository _lingoResourceRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public GetLingoResourcesInFormatQueryHandler(
            ILogger<GetLingoResourcesInFormatQueryHandler> logger,
            GetLingoResourcesInFormatQueryValidator validator,
            ILanguageResourcesRepository lingoAppRepository,
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

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();

                switch (request.Format.ToLower())
                {
                    case "json":

                        var result = await _lingoResourceRepository.GetLanguageResourcesInJSONFormat(
                            appId: request.AppId,
                            languageCode: request.LanguageCode);
                        return Response.BuildQueryRecordResponse<Dictionary<string, string>>().BuildSuccessResponse(result, authCtx?.RequestUri);

                    default:

                        return Response.BuildQueryRecordResponse<Dictionary<string, string>>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError("Format is not supported yet.", _authenticationContextProvider.GetAuthenticationContext().RequestUri));
                }
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
