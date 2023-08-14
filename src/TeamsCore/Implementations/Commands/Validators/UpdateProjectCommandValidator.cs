using BaseCore.Extensions;
using FluentValidation;
using TeamsCore.Declarations.Commands;
using TeamsCore.Declarations.Repositories;

namespace TeamsCore.Implementations.Commands.Validators
{
    public class UpdateProjectCommandValidator : AbstractValidator<UpdateProjectCommand>
    {

        private readonly IProjectRepository _projectRepository;
        private readonly IProductRepository _productRepository;
        public UpdateProjectCommandValidator(
            IProjectRepository projectRepository,
            IProductRepository productRepository)
        {
            _projectRepository = projectRepository;
            _productRepository = productRepository;

            RuleFor(x => x.ProjectId).NotNull().NotEmpty();

            RuleFor(x => x).MustAsync(BeAnExistingProject).WithMessage("Project doesn't exist.").When(x => !x.ProjectId.IsNullOrBlank());

            RuleFor(x => x).MustAsync(BeAnExistingProduct).WithMessage("Products doesn't exists.").When(x => x.LinkedProductIds != null);

        }

        private async Task<bool> BeAnExistingProject(UpdateProjectCommand command, CancellationToken arg2)
        {
            return await _projectRepository.BeAnExistingProject(command.ProjectId);
        }

        private async Task<bool> BeAnExistingProduct(UpdateProjectCommand command, CancellationToken arg2)
        {
            foreach (var productId in command.LinkedProductIds)
            {
                var exists = await _productRepository.BeAnExistingProductId(productId);

                if (!exists)
                    return false;
            }

            return true;
        }
    }

}
