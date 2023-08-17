using FluentValidation;
using Language.Domain.Repositories.Interfaces;

namespace Language.Application.Commands.Validators
{
    public class AddLingoAppCommandValidator : AbstractValidator<AddLingoAppCommand>
    {
        private readonly ILanguageAppRepository _lingoAppRepository;

        public AddLingoAppCommandValidator(ILanguageAppRepository lingoAppRepository)
        {
            _lingoAppRepository = lingoAppRepository;

            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Name is required");

            RuleFor(x => x.Name).MustAsync(NotBeAnExistingLingoApp).WithMessage("Lingo app already exists");
        }

        private async Task<bool> NotBeAnExistingLingoApp(string appName, CancellationToken token)
        {
            return !await _lingoAppRepository.BeAnExistingLingoApp(appName);
        }
    }
}
