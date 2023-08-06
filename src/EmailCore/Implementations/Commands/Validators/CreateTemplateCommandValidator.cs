using BaseCore.Extensions;
using EmailCore.Declarations.Commands;
using EmailCore.Declarations.Queries;
using EmailCore.Declarations.Repositories;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EmailCore.Implementations.Commands.Validators
{
    public class CreateTemplateCommandValidator : AbstractValidator<CreateTemplateCommand>
    {
        private readonly IEmailRepository _emailRepository;
        
        public CreateTemplateCommandValidator(IEmailRepository emailRepository)
        {
            _emailRepository = emailRepository;
           
            RuleFor(x => x.Name).NotNull().NotEmpty();
            RuleFor(x => x.Body).NotNull().NotEmpty();
            RuleFor(x => x.EmailTemplateType).NotNull().NotEmpty();
            RuleFor(x => x.Tags).NotNull().NotEmpty();

           // RuleFor(x => x.Name).MustAsync(NotBeAnExistingUserEmail).WithMessage("Email Template already exists.").When(x => !x.Name.IsNullOrBlank());
        }

        private async Task<bool> NotBeAnExistingUserEmail(string templateName, CancellationToken token)
        {
            return !await _emailRepository.BeAnExistingEmailTemplate(templateName);
        }


    }
}
