using Base.Application.Extensions;
using FluentValidation;
using Teams.ContentMangement.Domain.Repositories.Interfaces;

namespace Teams.ContentMangement.Application.Queries.Validators
{
    public class GetProjectsForProductIdQueryValidator : AbstractValidator<GetProjectsForProductIdQuery>
    {
        private readonly IProductRepository _productRepository;

        public GetProjectsForProductIdQueryValidator(IProductRepository productRepository)
        {
            _productRepository = productRepository;

            RuleFor(x => x.ProductId).NotNull().NotEmpty();
            RuleFor(x => x.ProductId)
                .MustAsync(BeAnExistingProductId)
                .WithMessage("Product does not exist").When(x => !x.ProductId.IsNullOrBlank());
        }

        private async Task<bool> BeAnExistingProductId(string productId, CancellationToken token)
        {
            return await _productRepository.BeAnExistingProductId(productId);
        }
    }
}
