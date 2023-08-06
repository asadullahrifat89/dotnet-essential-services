using BaseCore.Models.Responses;
using EmailCore.Declarations.Commands;
using EmailCore.Declarations.Repositories;
using EmailCore.Implementations.Commands.Validators;
using MediatR;
using Microsoft.Extensions.Logging;
using BaseCore.Extensions;
using BaseCore.Services;
using Amazon.Runtime.Internal;
using System.Threading;

namespace EmailCore.Implementations.Commands.Handlers
{
    public class UpdateTemplateCommandHandler : IRequestHandler<UpdateTemplateCommand, ServiceResponse>
    {
        private readonly ILogger<UpdateTemplateCommand> _logger;
        private readonly UpdateTemplateCommandValidator _validator;
        private readonly IEmailRepository _emailRepository;

        public UpdateTemplateCommandHandler(ILogger<UpdateTemplateCommand> logger, UpdateTemplateCommandValidator validator, IEmailRepository emailRepository)
        {
            _logger = logger;
            _validator = validator;
            _emailRepository = emailRepository;
        }

        public async Task<ServiceResponse> Handle(UpdateTemplateCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _emailRepository.UpdateTemplate (request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildServiceResponse().BuildErrorResponse(ex.Message);
            }
        }
    }
}
