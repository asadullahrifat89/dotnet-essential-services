using BaseCore.Models.Responses;
using EmailCore.Declarations.Commands;
using EmailCore.Declarations.Repositories;
using EmailCore.Implementations.Commands.Validators;
using MediatR;
using Microsoft.Extensions.Logging;
using BaseCore.Extensions;

namespace EmailCore.Implementations.Commands.Handlers
{
    public class UpdateEmailTemplateCommandHandler : IRequestHandler<UpdateEmailTemplateCommand, ServiceResponse>
    {
        private readonly ILogger<UpdateEmailTemplateCommand> _logger;
        private readonly UpdateEmailTemplateCommandValidator _validator;
        private readonly IEmailTemplateRepository _emailRepository;

        public UpdateEmailTemplateCommandHandler(
            ILogger<UpdateEmailTemplateCommand> logger,
            UpdateEmailTemplateCommandValidator validator,
            IEmailTemplateRepository emailRepository)
        {
            _logger = logger;
            _validator = validator;
            _emailRepository = emailRepository;
        }

        public async Task<ServiceResponse> Handle(UpdateEmailTemplateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _emailRepository.UpdateEmailTemplate(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildServiceResponse().BuildErrorResponse(ex.Message);
            }
        }
    }
}
