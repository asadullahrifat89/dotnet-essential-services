using FluentValidation;

namespace Teams.ContentMangement.Application.Queries.Validators
{
    public class GetProjectsQueryValidator : AbstractValidator<GetProjectsQuery>
    {
        public GetProjectsQueryValidator()
        {
            RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(0);
            RuleFor(x => x.PageSize).GreaterThan(0);

            RuleFor(x => x.PublishingStatus).IsInEnum().WithMessage("Invalid PublishingStatus.").When(x => x.PublishingStatus != null);
        }
    }
}
