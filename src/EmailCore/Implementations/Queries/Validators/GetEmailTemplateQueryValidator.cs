using BaseCore.Extensions;
using EmailCore.Declarations.Queries;
using EmailCore.Declarations.Repositories;
using FluentValidation;

namespace EmailCore.Implementations.Queries.Validators
{
    public class GetEmailTemplateQueryValidator : AbstractValidator<GetEmailTemplateQuery>
    {
        private readonly IEmailRepository _emailRepository;

        public GetEmailTemplateQueryValidator(IEmailRepository emailRepository)
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
