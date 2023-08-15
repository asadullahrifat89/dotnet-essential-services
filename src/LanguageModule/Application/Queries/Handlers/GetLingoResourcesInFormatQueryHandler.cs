using BaseModule.Application.DTOs.Responses;
using BaseModule.Infrastructure.Extensions;
using IdentityModule.Infrastructure.Services.Interfaces;
using LanguageModule.Application.Quaries;
using LanguageModule.Application.Quaries.Validators;
using LanguageModule.Domain.Repositories.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LanguageModule.Application.Quaries.Handlers
{
    public class GetLingoResourcesInFormatQueryHandler : IRequestHandler<GetLingoResourcesInFormatQuery, QueryRecordResponse<Dictionary<string, string>>>
    {
        #region Fields

        private readonly ILogger<GetLingoResourcesInFormatQueryHandler> _logger;
        private readonly GetLingoResourcesInFormatQueryValidator _validator;
        private readonly ILingoResourcesRepository _lingoResourceRepository;
        private readonly IAuthenticationContextProviderService _authenticationContext;

        #endregion

        #region Ctor

        public GetLingoResourcesInFormatQueryHandler(
            ILogger<GetLingoResourcesInFormatQueryHandler> logger,
            GetLingoResourcesInFormatQueryValidator validator,
            ILingoResourcesRepository lingoAppRepository,
            IAuthenticationContextProviderService authenticationContext)
        {
            _logger = logger;
            _validator = validator;
            _lingoResourceRepository = lingoAppRepository;
            _authenticationContext = authenticationContext;
        }

        #endregion

        #region Methods
        public async Task<QueryRecordResponse<Dictionary<string, string>>> Handle(GetLingoResourcesInFormatQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _lingoResourceRepository.GetLingoResourcesInFormat(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordResponse<Dictionary<string, string>>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContext.GetAuthenticationContext().RequestUri));
            }
        }

        #endregion
    }
}
