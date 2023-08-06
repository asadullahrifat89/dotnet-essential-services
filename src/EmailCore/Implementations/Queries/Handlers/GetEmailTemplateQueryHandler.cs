using BaseCore.Models.Responses;
using EmailCore.Models.Entities;
using MediatR;
using EmailCore.Declarations.Queries;
using EmailCore.Declarations.Repositories;
using BaseCore.Services;
using Microsoft.Extensions.Logging;
using EmailCore.Implementations.Queries.Validators;
using BaseCore.Extensions;

namespace EmailCore.Implementations.Queries.Handlers
{
    public class GetEmailTemplateQueryHandler : IRequestHandler<GetEmailTemplateQuery, QueryRecordResponse<EmailTemplate>>
    {
        private readonly IEmailRepository _emailRepository;
        private readonly IAuthenticationContextProvider _authenticationContext;
        private readonly ILogger<GetEmailTemplateQueryHandler> _logger;
        private readonly GetEmailTemplateQueryValidator _validator;

        public GetEmailTemplateQueryHandler(
            ILogger<GetEmailTemplateQueryHandler> logger,
            GetEmailTemplateQueryValidator validator,
            IEmailRepository emailRepository,
            IAuthenticationContextProvider authenticationContext)
        {
             _logger = logger;
            _validator = validator;
            _emailRepository = emailRepository;
            _authenticationContext = authenticationContext;
        }

        public async Task<QueryRecordResponse<EmailTemplate>> Handle(GetEmailTemplateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _emailRepository.GetEmailTemplate(request);
            }
            catch(Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordResponse<EmailTemplate>().BuildErrorResponse(
                                                          Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContext.GetAuthenticationContext().RequestUri));
            }
        }
    }
}
