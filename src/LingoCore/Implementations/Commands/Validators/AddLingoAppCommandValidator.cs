using FluentValidation;
using LingoCore.Declarations.Commands;
using LingoCore.Declarations.Repositories;
using SharpCompress.Archives;

namespace LingoCore.Implementations.Commands.Validators
{
    public class AddLingoAppCommandValidator: AbstractValidator<AddLingoAppCommand>
    {
        private readonly ILingoResourcesRepository _lingoResourcesRepository;

        public AddLingoAppCommandValidator(ILingoResourcesRepository lingoResourcesRepository)
        {
            _lingoResourcesRepository = lingoResourcesRepository;

            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Name is required");

            RuleFor(x => x.Name).MustAsync(NotBeAnExistingLingoApp).WithMessage("Lingo app already exists");
        }

        private async Task<bool> NotBeAnExistingLingoApp(string appName, CancellationToken token)
        {
            return !await _lingoResourcesRepository.BeAnExistingLingApp(appName);
        }
    }
}
