using BaseCore.Extensions;
using EmailCore.Declarations.Commands;
using EmailCore.Declarations.Repositories;
using FluentValidation;

namespace EmailCore.Implementations.Commands.Validators
{
    public class EnqueueEmailMessageCommandValidator : AbstractValidator<EnqueueEmailMessageCommand>
    {
        private readonly IEmailTemplateRepository _emailRepository;

        public EnqueueEmailMessageCommandValidator(IEmailTemplateRepository emailRepository)
        {
            _emailRepository = emailRepository;

            RuleFor(x => x.To).NotNull().NotEmpty();
            RuleFor(x => x.Body).NotNull().NotEmpty();
            RuleFor(x => x.Subject).NotNull().NotEmpty();
            RuleFor(x => x.Body).NotNull().NotEmpty();
            
            RuleFor(x => x.EmailTemplateId).MustAsync(BeAnExistingEmailTemplateById).WithMessage("Email Template doesn't exists.").When(x => !x.EmailTemplateId.IsNullOrBlank());
        }

        private async Task<bool> BeAnExistingEmailTemplateById(string emailTemplateId, CancellationToken token)
        {
            return await _emailRepository.BeAnExistingEmailTemplateById(emailTemplateId);
        }
    }
}
