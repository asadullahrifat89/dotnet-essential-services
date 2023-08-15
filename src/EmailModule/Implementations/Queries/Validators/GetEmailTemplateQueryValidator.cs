using BaseModule.Extensions;
using EmailModule.Declarations.Queries;
using EmailModule.Declarations.Repositories;
using FluentValidation;

namespace EmailModule.Implementations.Queries.Validators
{
    public class GetEmailTemplateQueryValidator : AbstractValidator<GetEmailTemplateQuery>
    {
        private readonly IEmailTemplateRepository _emailRepository;

        public GetEmailTemplateQueryValidator(IEmailTemplateRepository emailRepository)
        {
            _emailRepository = emailRepository;

            RuleFor(x => x.TemplateId).NotNull().NotEmpty();
            RuleFor(x => x.TemplateId).MustAsync(BeAnExistingEmailTemplateById).WithMessage("Template doesn't exist.").When(x => !x.TemplateId.IsNullOrBlank());
        }

        private Task<bool> BeAnExistingEmailTemplateById(string templateId, CancellationToken arg2)
        {
            return _emailRepository.BeAnExistingEmailTemplateById(templateId);
        }
    }
}
