using BaseCore.Extensions;
using BaseCore.Models.Responses;
using MediatR;
using Microsoft.Extensions.Logging;
using TeamsCore.Declarations.Commands;
using TeamsCore.Declarations.Repositories;
using TeamsCore.Implementations.Commands.Validators;

namespace TeamsCore.Implementations.Commands.Handlers
{
    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<UpdateProjectCommandHandler> _logger;
        private readonly UpdateProjectCommandValidator _validator;
        private readonly IProjectRepository _projectRepository;

        #endregion

        #region Ctor

        public UpdateProjectCommandHandler(
            ILogger<UpdateProjectCommandHandler> logger,
            UpdateProjectCommandValidator validator,
            IProjectRepository projectRepository)
        {
            _logger = logger;
            _validator = validator;
            _projectRepository = projectRepository;
        }

        #endregion

        #region Methods


        public async Task<ServiceResponse> Handle(UpdateProjectCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(request, cancellationToken);
                validationResult.EnsureValidResult();

                return await _projectRepository.UpdateProject(request);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildServiceResponse().BuildErrorResponse(ex.Message);
            }
        }

        #endregion
    }
}
