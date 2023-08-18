using Base.Application.Extensions;
using FluentValidation;
using Teams.ContentMangement.Domain.Repositories.Interfaces;

namespace Teams.ContentMangement.Application.Commands.Validators
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly IProductSearchCriteriaRepository _searchCriteriaRepository;

        public UpdateProductCommandValidator(IProductRepository productRepository, IProductSearchCriteriaRepository searchCriteriaRepository)
        {
            _productRepository = productRepository;
            _searchCriteriaRepository = searchCriteriaRepository;

            RuleFor(x => x.ProductId).NotNull().NotEmpty();
            RuleFor(x => x.ProductId)
                .MustAsync(BeAnExistingProductId)
                .WithMessage("Product does not exist").When(x => !x.ProductId.IsNullOrBlank());

            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.Description).NotNull().NotEmpty();
            RuleFor(x => x.ManPower).NotNull().NotEmpty();
            RuleFor(x => x.EmploymentTypes).NotNull().NotEmpty();
            RuleFor(x => x.IconUrl).NotNull().NotEmpty();
            RuleFor(x => x.Experience).NotNull().NotEmpty();
            RuleFor(x => x.ProductCostType).NotNull().NotEmpty().IsInEnum();
            RuleFor(x => x.PublishingStatus).IsInEnum();

            RuleFor(x => x.LinkedProductSearchCriteriaIds)
                .MustAsync(BeAnExistingSearchCriteriaId)
                .WithMessage("Search criteria does not exist.");
        }

        private async Task<bool> BeAnExistingSearchCriteriaId(string[] LinkedSearchCriteriaIds, CancellationToken token)
        {
            foreach (var searchCriteriaId in LinkedSearchCriteriaIds)
            {
                var exists = await _searchCriteriaRepository.BeAnExistingProductSearchCriteriaById(searchCriteriaId);

                if (!exists)
                    return false;
            }

            return true;
        }

        private async Task<bool> BeAnExistingProductId(string productId, CancellationToken token)
        {
            return await _productRepository.BeAnExistingProductId(productId);
        }
    }
}
