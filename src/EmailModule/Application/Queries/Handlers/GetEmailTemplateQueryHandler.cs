using MediatR;
using Microsoft.Extensions.Logging;
using BaseModule.Infrastructure.Extensions;
using BaseModule.Application.DTOs.Responses;
using EmailModule.Domain.Repositories.Interfaces;
using EmailModule.Domain.Entities;
using EmailModule.Application.Queries.Validators;
using IdentityModule.Infrastructure.Services.Interfaces;

namespace EmailModule.Application.Queries.Handlers
{
    public class GetEmailTemplateQueryHandler : IRequestHandler<GetEmailTemplateQuery, QueryRecordResponse<EmailTemplate>>
    {
        private readonly IEmailTemplateRepository _emailRepository;
        private readonly IAuthenticationContextProviderService _authenticationContext;
        private readonly ILogger<GetEmailTemplateQueryHandler> _logger;
        private readonly GetEmailTemplateQueryValidator _validator;

        public GetEmailTemplateQueryHandler(
            ILogger<GetEmailTemplateQueryHandler> logger,
            GetEmailTemplateQueryValidator validator,
            IEmailTemplateRepository emailRepository,
            IAuthenticationContextProviderService authenticationContext)
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
