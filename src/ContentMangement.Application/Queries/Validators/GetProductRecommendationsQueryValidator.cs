using FluentValidation;
using Teams.ContentMangement.Domain.Repositories.Interfaces;

namespace Teams.ContentMangement.Application.Queries.Validators
{
    public class GetProductRecommendationsQueryValidator : AbstractValidator<GetProductRecommendationsQuery>
    {
        private readonly IProductSearchCriteriaRepository _productSearchCriteriaRepository;

        public GetProductRecommendationsQueryValidator(IProductSearchCriteriaRepository productSearchCriteriaRepository)
        {
            _productSearchCriteriaRepository = productSearchCriteriaRepository;

            RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(0);
            RuleFor(x => x.PageSize).GreaterThan(0);

            RuleFor(x => x.ProductSearchCriteriaIds).NotNull();
            RuleFor(x => x).MustAsync(BeAnExistingSearchCriteriaId).WithMessage("One or more product search critera doesn't exist.").When(x => x.ProductSearchCriteriaIds is not null);

            RuleFor(x => x.MinimumManPower).GreaterThan(0).When(x => x.MinimumManPower.HasValue).WithMessage("Invalid ManPower");
            RuleFor(x => x.MinimumExperience).GreaterThan(0).When(x => x.MinimumExperience.HasValue).WithMessage("Invalid Experience");
        }

        private async Task<bool> BeAnExistingSearchCriteriaId(GetProductRecommendationsQuery query, CancellationToken arg2)
        {
            foreach (var id in query.ProductSearchCriteriaIds)
            {
                var exists = await _productSearchCriteriaRepository.BeAnExistingProductSearchCriteriaById(id);

                if (!exists)
                    return false;
            }

            return true;
        }
    }
}
