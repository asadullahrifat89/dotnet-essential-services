using Base.Application.Extensions;
using FluentValidation;
using Teams.ContentMangement.Domain.Repositories.Interfaces;

namespace Teams.ContentMangement.Application.Queries.Validators
{
    public class GetProjectQueryValidator : AbstractValidator<GetProjectQuery>
    {
        private readonly IProjectRepository _projectRepository;

        public GetProjectQueryValidator(IProjectRepository projectRepository)
        {
            _projectRepository = projectRepository;

            RuleFor(x => x.ProjectId).NotNull().NotEmpty();
            RuleFor(x => x.ProjectId).MustAsync(BeAnExistingProject).WithMessage("Project doesn't exist.").When(x => !x.ProjectId.IsNullOrBlank());
        }

        private async Task<bool> BeAnExistingProject(string projectId, CancellationToken arg2)
        {
            return await _projectRepository.BeAnExistingProject(projectId);
        }
    }
}
