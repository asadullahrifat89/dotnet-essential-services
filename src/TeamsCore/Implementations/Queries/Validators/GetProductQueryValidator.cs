using BaseCore.Extensions;
using FluentValidation;
using TeamsCore.Declarations.Queries;
using TeamsCore.Declarations.Repositories;

namespace TeamsCore.Implementations.Queries.Validators
{
    public class GetProductQueryValidator : AbstractValidator<GetProductQuery>
    {
        private readonly IProductRepository _productRepository;

        public GetProductQueryValidator(IProductRepository productRepository)
        {
            _productRepository = productRepository;

            RuleFor(x => x.ProductId).NotNull().NotEmpty();
            RuleFor(x => x.ProductId).MustAsync(BeAnExistingProduct).WithMessage("Product doesn't exist.").When(x => !x.ProductId.IsNullOrBlank());
        }

        private async Task<bool> BeAnExistingProduct(string productId, CancellationToken arg2)
        {
            return await _productRepository.BeAnExistingProductId(productId);
        }
    }
}
