using FluentValidation;
using TeamsCore.Declarations.Queries;

namespace TeamsCore.Implementations.Queries.Validators
{
    public class GetProjectsQueryValidator : AbstractValidator<GetProjectsQuery>
    {
        public GetProjectsQueryValidator()
        {
            RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(0);
            RuleFor(x => x.PageSize).GreaterThan(0);
        }
    }
}
