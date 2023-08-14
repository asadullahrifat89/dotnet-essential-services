using BaseCore.Extensions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsCore.Declarations.Queries;
using TeamsCore.Declarations.Repositories;

namespace TeamsCore.Implementations.Queries.Validators
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
