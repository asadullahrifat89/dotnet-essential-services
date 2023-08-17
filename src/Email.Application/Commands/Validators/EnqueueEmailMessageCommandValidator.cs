using Base.Application.Extensions;
using Email.Application.Commands;
using Email.Domain.Entities;
using Email.Domain.Repositories.Interfaces;
using FluentValidation;
using System.Text.RegularExpressions;

namespace Email.Application.Commands.Validators
{
    public class EnqueueEmailMessageCommandValidator : AbstractValidator<EnqueueEmailMessageCommand>
    {
        private readonly IEmailTemplateRepository _emailRepository;

        public EnqueueEmailMessageCommandValidator(IEmailTemplateRepository emailRepository)
        {
            _emailRepository = emailRepository;

            RuleFor(x => x.To).NotNull().NotEmpty();
            RuleFor(x => x.To).Must(x => x.Length > 0).WithMessage("To can not be empty.").When(x => x.To is not null);
            RuleFor(x => x.To).Must(HaveValidEmail).WithMessage("To contains invalid email address.").When(x => x.To is not null && x.To.Any());

            RuleFor(x => x.CC).Must(HaveValidEmail).WithMessage("CC contains invalid email address.").When(x => x.CC is not null && x.CC.Any());
            RuleFor(x => x.BCC).Must(HaveValidEmail).WithMessage("BCC contains invalid email address.").When(x => x.BCC is not null && x.BCC.Any());

            RuleFor(x => x.Subject).NotNull().NotEmpty();

            RuleFor(x => x.EmailTemplateConfiguration.EmailTemplateId).MustAsync(BeAnExistingEmailTemplateById).WithMessage("Email Template doesn't exists.").When(x => x.EmailTemplateConfiguration != null && !x.EmailTemplateConfiguration.EmailTemplateId.IsNullOrBlank());

            RuleFor(x => x.EmailBodyType).NotNull();
            RuleFor(x => x.EmailBodyType).Must(x => x == EmailBodyType.NonTemplated || x == EmailBodyType.Templated).WithMessage("Invalid email body type.");

            RuleFor(x => x).Must(x => !x.EmailTemplateConfiguration.EmailTemplateId.IsNullOrBlank()).WithMessage("Pls send an email template id if body is empty.").When(x => x.EmailBody.Content.IsNullOrBlank() || x.EmailBodyType == EmailBodyType.Templated);
            RuleFor(x => x).Must(x => !x.EmailBody.Content.IsNullOrBlank()).WithMessage("Pls send an email body id if email template is empty.").When(x => x.EmailTemplateConfiguration.EmailTemplateId.IsNullOrBlank() || x.EmailBodyType == EmailBodyType.NonTemplated);
            RuleFor(x => x).Must(x => x.EmailBody.Content.IsNullOrBlank() || x.EmailTemplateConfiguration.EmailTemplateId.IsNullOrBlank()).WithMessage("Pls send either an email body or an email template id and not both.").When(x => !x.EmailBody.Content.IsNullOrBlank() && !x.EmailTemplateConfiguration.EmailTemplateId.IsNullOrBlank());
        }

        private bool HaveValidEmail(EmailContact[] emailContacts)
        {
            return emailContacts.All(x => IsValidEmail(x.Email));
        }

        private async Task<bool> BeAnExistingEmailTemplateById(string emailTemplateId, CancellationToken token)
        {
            return await _emailRepository.BeAnExistingEmailTemplateById(emailTemplateId);
        }

        public bool IsValidEmail(string email)
        {
            // Regular expression pattern for validating email addresses
            string pattern = @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$";

            // Use Regex.IsMatch to check if the email matches the pattern
            return Regex.IsMatch(email, pattern);
        }
    }
}
