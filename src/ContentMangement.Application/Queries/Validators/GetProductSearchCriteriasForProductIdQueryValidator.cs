using Base.Application.Extensions;
using FluentValidation;
using Teams.ContentMangement.Domain.Repositories.Interfaces;

namespace Teams.ContentMangement.Application.Queries.Validators
{
    public class GetProductSearchCriteriasForProductIdQueryValidator : AbstractValidator<GetProductSearchCriteriasForProductIdQuery>
    {
        private readonly IProductSearchCriteriaRepository _productSearchCriteriaRepository;
        private readonly IProductRepository _productRepository;

        public GetProductSearchCriteriasForProductIdQueryValidator(IProductSearchCriteriaRepository productSearchCriteriaRepository, IProductRepository productRepository)
        {
            _productSearchCriteriaRepository = productSearchCriteriaRepository;
            _productRepository = productRepository;

            RuleFor(x => x.ProductId).NotNull().NotEmpty().WithMessage("Product Id must not be empty");
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
