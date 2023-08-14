using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsCore.Declarations.Commands;
using TeamsCore.Declarations.Repositories;
using TeamsCore.Implementations.Repositories;

namespace TeamsCore.Implementations.Commands.Validators
{
    public class AddProjectCommandValidator : AbstractValidator<AddProjectCommand>
    {
        private readonly IProductRepository _productRepository;
        public AddProjectCommandValidator(IProductRepository productRepository)
        {
            _productRepository = productRepository;
            RuleFor(x => x.Name).NotEmpty().WithMessage("Name is required.");
            RuleFor(x => x.Description).NotEmpty().WithMessage("Description is required.");
            RuleFor(x => x.Link).NotEmpty().WithMessage("Link is required.");
            RuleFor(x => x.IconUrl).NotEmpty().WithMessage("IconUrl is required.");

            RuleFor(x => x.LinkedProductIds).NotEmpty().WithMessage("LinkedProductIds is required.");

            // be an existing productID
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
