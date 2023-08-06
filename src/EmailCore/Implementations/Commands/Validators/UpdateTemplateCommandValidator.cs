using BaseCore.Extensions;
using EmailCore.Declarations.Commands;
using EmailCore.Declarations.Repositories;
using FluentValidation;

namespace EmailCore.Implementations.Commands.Validators
{
    public class UpdateTemplateCommandValidator : AbstractValidator<UpdateTemplateCommand>
    {
        private readonly IEmailRepository _emailRepository;

        public UpdateTemplateCommandValidator(IEmailRepository emailRepository)
        {
            _emailRepository = emailRepository;

            RuleFor(x => x.TemplateId).MustAsync(BeAnExistingTemplate).WithMessage("Template doesn't exist.").When(x => !x.TemplateId.IsNullOrBlank());
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Body).NotNull().NotEmpty();
            RuleFor(x => x.Tags).NotNull().NotEmpty();
        }

        private async Task<bool> BeAnExistingTemplate(string templateId, CancellationToken arg2)
        {
            return await _emailRepository.BeAnExistingEmailTemplateById(templateId);
        }
    }
}
