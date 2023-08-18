using FluentValidation;
using Teams.ContentMangement.Domain.Repositories.Interfaces;

namespace Teams.ContentMangement.Application.Commands.Validators
{
    public class AddProjectCommandValidator : AbstractValidator<AddProjectCommand>
    {
        private readonly IProductRepository _productRepository;
        public AddProjectCommandValidator(IProductRepository productRepository)
        {
            _productRepository = productRepository;

            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Description).NotNull().NotEmpty();
            RuleFor(x => x.IconUrl).NotNull().NotEmpty();                        
            RuleFor(x => x.LinkedProductIds).NotEmpty().WithMessage("LinkedProductIds is required.");           
            RuleFor(x => x).MustAsync(BeAnExistingProductId).WithMessage("Product Id doesn't exist.").When(x => x.LinkedProductIds != null);
        }

        private async Task<bool> BeAnExistingProductId(AddProjectCommand command, CancellationToken arg2)
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
