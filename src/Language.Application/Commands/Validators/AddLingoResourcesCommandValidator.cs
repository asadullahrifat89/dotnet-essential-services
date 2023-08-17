using Base.Application.Extensions;
using FluentValidation;
using LanguageModule.Domain.Repositories.Interfaces;

namespace LanguageModule.Application.Commands.Validators
{
    public class AddLingoResourcesCommandValidator : AbstractValidator<AddLingoResourcesCommand>
    {
        private readonly ILanguageAppRepository _lingoAppRepository;
        private readonly ILanguageResourcesRepository _languageResourcesRepository;

        public AddLingoResourcesCommandValidator(ILanguageAppRepository lingoAppRepository, ILanguageResourcesRepository languageResourcesRepository)
        {
            _lingoAppRepository = lingoAppRepository;
            _languageResourcesRepository = languageResourcesRepository;

            RuleFor(x => x.AppId).NotNull().NotEmpty().WithMessage("App Id is required");
            RuleFor(x => x.AppId).MustAsync(BeAnExistingLingoApp).WithMessage("Lingo app does not exist").When(x => !x.AppId.IsNullOrBlank());
            RuleFor(x => x.ResourceKeys).Must(x => x.Count < 11).WithMessage("You can not send more than 10 resources.").When(x => x.ResourceKeys is not null);
            RuleFor(x => x).MustAsync(NotBeAnExistingLanguageCodeAndResourceKey).WithMessage("The provided resource key for the language code already exists.").When(x => x.ResourceKeys is not null);
        }

        private async Task<bool> NotBeAnExistingLanguageCodeAndResourceKey(AddLingoResourcesCommand command, CancellationToken token)
        {
            foreach (var resourceKeyEntry in command.ResourceKeys)
            {
                foreach (var cultureValue in resourceKeyEntry.CultureValues)
                {
                    var exists = await _languageResourcesRepository.BeAnExistingLanguageCodeAndResourceKey(
                        languageCode: cultureValue.LanguageCode,
                        resourceKey: resourceKeyEntry.ResourceKey);

                    if (exists)
                        return false;
                }
            }

            return true;
        }

        private async Task<bool> BeAnExistingLingoApp(string appId, CancellationToken token)
        {
            return await _lingoAppRepository.BeAnExistingLingoAppById(appId);
        }
    }
}
