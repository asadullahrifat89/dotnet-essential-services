using FluentValidation;

namespace Identity.Application.Queries.Validators
{
    public class GetUsersQueryValidator : AbstractValidator<GetUsersQuery>
    {
        public GetUsersQueryValidator()
        {
            RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(0);
            RuleFor(x => x.PageSize).GreaterThan(0);
        }
    }
}
