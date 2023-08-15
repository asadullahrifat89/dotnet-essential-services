using BaseModule.Infrastructure.Extensions;
using EmailModule.Domain.Entities;
using EmailModule.Domain.Interfaces;
using FluentValidation;

namespace EmailModule.Application.Commands.Validators
{
    public class EnqueueEmailMessageCommandValidator : AbstractValidator<EnqueueEmailMessageCommand>
    {
        private readonly IEmailTemplateRepository _emailRepository;

        public EnqueueEmailMessageCommandValidator(IEmailTemplateRepository emailRepository)
        {
            _emailRepository = emailRepository;

            RuleFor(x => x.To).NotNull().NotEmpty();
            RuleFor(x => x.Subject).NotNull().NotEmpty();


            RuleFor(x => x.EmailTemplateConfiguration.EmailTemplateId).MustAsync(BeAnExistingEmailTemplateById).WithMessage("Email Template doesn't exists.").When(x => x.EmailTemplateConfiguration != null && !x.EmailTemplateConfiguration.EmailTemplateId.IsNullOrBlank());

            RuleFor(x => x.EmailBodyType).NotNull();
            RuleFor(x => x.EmailBodyType).Must(x => x == EmailBodyType.NonTemplated || x == EmailBodyType.Templated).WithMessage("Invalid email body type.");

            RuleFor(x => x).Must(x => !x.EmailTemplateConfiguration.EmailTemplateId.IsNullOrBlank()).WithMessage("Pls send an email template id if body is empty.").When(x => x.EmailBody.Content.IsNullOrBlank() || x.EmailBodyType == EmailBodyType.Templated);

            RuleFor(x => x).Must(x => !x.EmailBody.Content.IsNullOrBlank()).WithMessage("Pls send an email body id if email template is empty.").When(x => x.EmailTemplateConfiguration.EmailTemplateId.IsNullOrBlank() || x.EmailBodyType == EmailBodyType.NonTemplated);

            RuleFor(x => x).Must(x => x.EmailBody.Content.IsNullOrBlank() || x.EmailTemplateConfiguration.EmailTemplateId.IsNullOrBlank()).WithMessage("Pls send either an email body or an email template id and not both.").When(x => !x.EmailBody.Content.IsNullOrBlank() && !x.EmailTemplateConfiguration.EmailTemplateId.IsNullOrBlank());
        }

        private async Task<bool> BeAnExistingEmailTemplateById(string emailTemplateId, CancellationToken token)
        {
            return await _emailRepository.BeAnExistingEmailTemplateById(emailTemplateId);
        }
    }
}
