using Base.Shared.Constants;
using FluentValidation;

namespace Teams.UserManagement.Application.Commands.Validators
{
    public class OnboardUserCommandValidator : AbstractValidator<OnboardUserCommand>
    {
        public OnboardUserCommandValidator()
        {
            RuleFor(x => x.Email).NotNull().NotEmpty().WithMessage("Email must not be empty");
            RuleFor(x => x.MetaTags).NotNull().NotEmpty().Must(x => x.Length > 0).WithMessage("MetaTags must not be empty.");
            RuleForEach(x => x.MetaTags).Must(x => CommonConstants.OnboardUserMetaTags.Contains(x)).WithMessage("Invalid meta tags.").When(x => x.MetaTags is not null && x.MetaTags.Any());
        }
    }
}
