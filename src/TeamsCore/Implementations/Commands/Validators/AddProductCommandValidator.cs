using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsCore.Declarations.Commands;
using TeamsCore.Declarations.Repositories;

namespace TeamsCore.Implementations.Commands.Validators
{
    public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
    {
        private readonly IProductRepository _productRepository;
        public AddProductCommandValidator(IProductRepository productRepository)
        {
            _productRepository = productRepository;
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
