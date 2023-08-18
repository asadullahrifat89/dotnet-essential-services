using FluentValidation;

namespace Teams.CustomerEngagement.Application.Queries.Validators
{
    public class GetQuotationsQueryValidator : AbstractValidator<GetQuotationsQuery>
    {
        public GetQuotationsQueryValidator()
        {
            RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(0);
            RuleFor(x => x.PageSize).GreaterThan(0);
            RuleFor(x => x.Priority).IsInEnum().When(x => x.Priority != null);
            RuleFor(x => x.FromDate).Must(x => x < DateTime.MaxValue && x > DateTime.MinValue).When(x => x.FromDate != null);
            RuleFor(x => x.ToDate).Must(x => x < DateTime.MaxValue && x > DateTime.MinValue).When(x => x.ToDate != null);
            RuleFor(x => x).Must(x => x.ToDate >= x.FromDate).WithMessage("To date can not be less than from date.").When(x => x.FromDate != null && x.ToDate != null);
            RuleFor(x => x).Must(x => x.FromDate <= x.ToDate).WithMessage("From date can not be greater than to date.").When(x => x.FromDate != null && x.ToDate != null);
        }
    }
}
