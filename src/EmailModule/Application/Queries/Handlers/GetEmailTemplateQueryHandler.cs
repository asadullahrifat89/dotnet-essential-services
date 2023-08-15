using MediatR;
using Microsoft.Extensions.Logging;
using BaseModule.Infrastructure.Extensions;
using BaseModule.Application.DTOs.Responses;
using EmailModule.Domain.Entities;
using EmailModule.Application.Queries.Validators;
using EmailModule.Domain.Repositories.Interfaces;
using IdentityModule.Application.Providers.Interfaces;

namespace EmailModule.Application.Queries.Handlers
{
    public class GetEmailTemplateQueryHandler : IRequestHandler<GetEmailTemplateQuery, QueryRecordResponse<EmailTemplate>>
    {
        private readonly IEmailTemplateRepository _emailRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;
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
            _authenticationContextProvider = authenticationContext;
        }

        public async Task<QueryRecordResponse<EmailTemplate>> Handle(GetEmailTemplateQuery request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _emailRepository.GetEmailTemplate(request.TemplateId);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildQueryRecordResponse<EmailTemplate>().BuildErrorResponse(Response.BuildErrorResponse().BuildExternalError(ex.Message, _authenticationContextProvider.GetAuthenticationContext().RequestUri));
            }
        }
    }
}
