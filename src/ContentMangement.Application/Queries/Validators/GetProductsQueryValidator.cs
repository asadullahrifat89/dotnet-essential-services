using FluentValidation;

namespace Teams.ContentMangement.Application.Queries.Validators
{
    public class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
    {
        public GetProductsQueryValidator()
        {
            RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(0);
            RuleFor(x => x.PageSize).GreaterThan(0);

            RuleFor(x => x.ProductCostType).IsInEnum().When(x => x.ProductCostType.HasValue).WithMessage("Invalid ProductCostType");
            RuleFor(x => x.EmploymentType).IsInEnum().When(x => x.EmploymentType.HasValue).WithMessage("Invalid EmploymentType");
            RuleFor(x => x.PublishingStatus).IsInEnum().When(x => x.PublishingStatus.HasValue).WithMessage("Invalid PublishingStatus");
            RuleFor(x => x.ManPower).GreaterThan(0).When(x => x.ManPower.HasValue).WithMessage("Invalid ManPower");
            RuleFor(x => x.Experience).GreaterThan(0).When(x => x.Experience.HasValue).WithMessage("Invalid Experience");
        }
    }
}
