using ContentMangement.Domain.Entities;
using FluentValidation;

namespace ContentMangement.Application.Queries.Validators
{
    public class GetProductSearchCriteriasQueryValidator : AbstractValidator<GetProductSearchCriteriasQuery>
    {
        public GetProductSearchCriteriasQueryValidator()
        {
            RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(0);
            RuleFor(x => x.PageSize).GreaterThan(0);

            RuleFor(x => x.SearchCriteriaType)
                .Must(BeAnExistingProductSearchCriteriaType)
                .When(x => x.SearchCriteriaType.HasValue)
                .WithMessage("Invalid ProductSearchCriteriaType");

            RuleFor(x => x.SkillsetType)
                .Must(BeAnExistingSkillsetType)
                .When(x => x.SkillsetType.HasValue)
                .WithMessage("Invalid SkillsetType");

        }

        private bool BeAnExistingProductSearchCriteriaType(SearchCriteriaType? productSearchCriteriaType)
        {
            return Enum.IsDefined(typeof(SearchCriteriaType), productSearchCriteriaType);
        }

        private bool BeAnExistingSkillsetType(SkillsetType? skillsetType)
        {
            return Enum.IsDefined(typeof(SkillsetType), skillsetType);
        }
    }
}
