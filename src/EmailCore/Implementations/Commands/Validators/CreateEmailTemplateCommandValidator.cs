using BaseCore.Extensions;
using EmailCore.Declarations.Commands;
using EmailCore.Declarations.Repositories;
using EmailCore.Models.Entities;
using FluentValidation;

namespace EmailCore.Implementations.Commands.Validators
{
    public class CreateEmailTemplateCommandValidator : AbstractValidator<CreateEmailTemplateCommand>
    {
        private readonly IEmailTemplateRepository _emailRepository;

        public CreateEmailTemplateCommandValidator(IEmailTemplateRepository emailRepository)
        {
            _emailRepository = emailRepository;

            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Body).NotNull().NotEmpty();
            RuleFor(x => x.Tags).NotNull().NotEmpty();

            RuleFor(x => x.EmailBodyContentType).NotNull();
            RuleFor(x => x.EmailBodyContentType).Must(x => x == EmailBodyContentType.Text || x == EmailBodyContentType.HTML).WithMessage("Invalid email template type.").When(x => x.EmailBodyContentType != null);

            RuleFor(x => x.Name).MustAsync(NotBeAnExistingUserEmailTemplate).WithMessage("Email Template already exists.").When(x => !x.Name.IsNullOrBlank());
        }

        private async Task<bool> NotBeAnExistingUserEmailTemplate(string name, CancellationToken token)
        {
            return !await _emailRepository.BeAnExistingEmailTemplate(name);
        }
    }
}
