using BaseCore.Extensions;
using BaseCore.Models.Responses;
using BaseCore.Services;
using LingoCore.Declarations.Queries;
using LingoCore.Declarations.Repositories;
using LingoCore.Implementations.Queries.Validators;
using MediatR;
using Microsoft.Extensions.Logging;

namespace LingoCore.Implementations.Queries.Handlers
{
    public class GetLingoResourcesInFormatQueryHandler : IRequestHandler<GetLingoResourcesInFormatQuery, QueryRecordResponse<Dictionary<string, string>>>
    {

        #region Fields

        private readonly ILogger<GetLingoResourcesInFormatQueryHandler> _logger;
        private readonly GetLingoResourcesInFormatQueryValidator _validator;
        private readonly ILingoResourcesRepository _lingoResourceRepository;
        private readonly IAuthenticationContextProvider _authenticationContext;

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
