using MediatR;
using Microsoft.Extensions.Logging;
using EmailModule.Declarations.Queries;
using BaseModule.Models.Responses;
using BaseModule.Services;
using EmailModule.Models.Entities;
using EmailModule.Implementations.Queries.Validators;
using BaseModule.Extensions;
using EmailModule.Declarations.Repositories;

namespace EmailModule.Implementations.Queries.Handlers
{
    public class GetEmailTemplateQueryHandler : IRequestHandler<GetEmailTemplateQuery, QueryRecordResponse<EmailTemplate>>
    {
        private readonly IEmailTemplateRepository _emailRepository;
        private readonly IAuthenticationContextProvider _authenticationContext;
        private readonly ILogger<GetEmailTemplateQueryHandler> _logger;
        private readonly GetEmailTemplateQueryValidator _validator;

        public GetEmailTemplateQueryHandler(
            ILogger<GetEmailTemplateQueryHandler> logger,
            GetEmailTemplateQueryValidator validator,
            IEmailTemplateRepository emailRepository,
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
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordResponse<EmailTemplate>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContext.GetAuthenticationContext().RequestUri));
            }
        }
    }
}
