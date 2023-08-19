using Base.Application.DTOs.Responses;
using Base.Application.Extensions;
using Identity.Application.Providers.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;
using Teams.ContentMangement.Application.Commands.Validators;
using Teams.ContentMangement.Domain.Repositories.Interfaces;

namespace Teams.ContentMangement.Application.Commands.Handlers
{
    public class UpdateProjectCommandHandler : IRequestHandler<UpdateProjectCommand, ServiceResponse>
    {
        #region Fields

        private readonly ILogger<UpdateProjectCommandHandler> _logger;
        private readonly UpdateProjectCommandValidator _validator;
        private readonly IProjectRepository _projectRepository;
        private readonly IAuthenticationContextProvider _authenticationContextProvider;

        #endregion

        #region Ctor

        public UpdateProjectCommandHandler(
            ILogger<UpdateProjectCommandHandler> logger,
            UpdateProjectCommandValidator validator,
            IProjectRepository projectRepository,
            IAuthenticationContextProvider authenticationContextProvider)
        {
            _logger = logger;
            _validator = validator;
            _projectRepository = projectRepository;
            _authenticationContextProvider = authenticationContextProvider;
        }

        #endregion

        #region Methods


        public async Task<ServiceResponse> Handle(UpdateProjectCommand command, CancellationToken cancellationToken)
        {
            try
            {
                var validationResult = await _validator.ValidateAsync(command, cancellationToken);
                validationResult.EnsureValidResult();

                var authCtx = _authenticationContextProvider.GetAuthenticationContext();
                var project = UpdateProjectCommand.Map(command, authCtx);

                var result = await _projectRepository.UpdateProject(project, command.LinkedProductIds);
                return Response.BuildServiceResponse().BuildSuccessResponse(result, authCtx.RequestUri);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                return Response.BuildServiceResponse().BuildErrorResponse(ex.Message, _authenticationContextProvider.GetAuthenticationContext().RequestUri);
            }
        }

        #endregion
    }
}
