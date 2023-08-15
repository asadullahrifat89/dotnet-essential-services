using BaseModule.Infrastructure.Extensions;
using FluentValidation;
using LanguageModule.Application.Commands;
using LanguageModule.Domain.Repositories.Interfaces;

namespace LanguageModule.Application.Commands.Validators
{
    public class AddLingoResourcesCommandValidator : AbstractValidator<AddLingoResourcesCommand>
    {
        private readonly ILingoAppRepository _lingoAppRepository;

        public AddLingoResourcesCommandValidator(ILingoAppRepository lingoAppRepository)
        {
            _lingoAppRepository = lingoAppRepository;

            RuleFor(x => x.AppId).NotNull().NotEmpty().WithMessage("App Id is required");
            RuleFor(x => x.AppId).MustAsync(BeAnExistingLingoApp).WithMessage("Lingo app does not exist").When(x => !x.AppId.IsNullOrBlank());
            RuleFor(x => x.ResourceKeys).Must(x => x.Count < 11).WithMessage("You can not send more than 10 resources.").When(x => x.ResourceKeys is not null);
        }

        private async Task<bool> BeAnExistingLingoApp(string appId, CancellationToken token)
        {
            return await _lingoAppRepository.BeAnExistingLingoAppById(appId);
        }
    }
}
