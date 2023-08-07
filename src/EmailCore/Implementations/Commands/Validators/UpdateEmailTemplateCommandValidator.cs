using BaseCore.Extensions;
using EmailCore.Declarations.Commands;
using EmailCore.Declarations.Repositories;
using EmailCore.Models.Entities;
using FluentValidation;

namespace EmailCore.Implementations.Commands.Validators
{
    public class UpdateEmailTemplateCommandValidator : AbstractValidator<UpdateEmailTemplateCommand>
    {
        private readonly IEmailTemplateRepository _emailRepository;

        public UpdateEmailTemplateCommandValidator(IEmailTemplateRepository emailRepository)
        {
            _emailRepository = emailRepository;

            RuleFor(x => x.TemplateId).MustAsync(BeAnExistingTemplate).WithMessage("Template doesn't exist.").When(x => !x.TemplateId.IsNullOrBlank());
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Body).NotNull().NotEmpty();
            RuleFor(x => x.Tags).NotNull().NotEmpty();

            RuleFor(x => x.EmailBodyContentType).NotNull();
            RuleFor(x => x.EmailBodyContentType).Must(x => x == EmailBodyContentType.Text || x == EmailBodyContentType.HTML).WithMessage("Invalid email template type.").When(x => x.EmailBodyContentType != null);
        }

        private async Task<bool> BeAnExistingTemplate(string templateId, CancellationToken arg2)
        {
            return await _emailRepository.BeAnExistingEmailTemplateById(templateId);
        }
    }
}
