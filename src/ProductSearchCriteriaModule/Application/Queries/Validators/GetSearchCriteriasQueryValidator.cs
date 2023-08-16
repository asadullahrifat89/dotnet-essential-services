using FluentValidation;
using ProductSearchCriteriaModule.Domain.Entities;

namespace ProductSearchCriteriaModule.Application.Queries.Validators
{
    public class GetProductSearchCriteriasQueryValidator : AbstractValidator<GetProductSearchCriteriasQuery>
    {
        public GetProductSearchCriteriasQueryValidator()
        {
            RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(0);

            RuleFor(x => x.PageSize).GreaterThan(0);

            RuleFor(x => x.ProductSearchCriteriaType)
                .Must(BeAnExistingProductSearchCriteriaType)
                .When(x => x.ProductSearchCriteriaType.HasValue)
                .WithMessage("Invalid ProductSearchCriteriaType");

            RuleFor(x => x.SkillsetType)
                .Must(BeAnExistingSkillsetType)
                .When(x => x.SkillsetType.HasValue)
                .WithMessage("Invalid SkillsetType");

        }

        private bool BeAnExistingProductSearchCriteriaType(ProductSearchCriteriaType? ProductSearchCriteriaType)
        {
            return Enum.IsDefined(typeof(ProductSearchCriteriaType), ProductSearchCriteriaType);
        }

        private bool BeAnExistingSkillsetType(SkillsetType? skillsetType)
        {
            return Enum.IsDefined(typeof(SkillsetType), skillsetType);
        }
    }
}
