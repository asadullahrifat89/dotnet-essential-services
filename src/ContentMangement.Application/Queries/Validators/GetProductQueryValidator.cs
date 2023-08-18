using Base.Application.Extensions;
using FluentValidation;
using Teams.ContentMangement.Domain.Repositories.Interfaces;

namespace Teams.ContentMangement.Application.Queries.Validators
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
