using BaseModule.Infrastructure.Extensions;
using FluentValidation;
using LanguageModule.Declarations.Queries;
using LanguageModule.Declarations.Repositories;

namespace LanguageModule.Implementations.Queries.Validators
{
    public class GetLingoResourcesInFormatQueryValidator : AbstractValidator<GetLingoResourcesInFormatQuery>
    {
        private readonly ILingoResourcesRepository _lingoResourcesRepository;

        private readonly ILingoAppRepository _lingoAppRepository;

        public GetLingoResourcesInFormatQueryValidator(ILingoResourcesRepository lingoResourcesRepository, ILingoAppRepository lingoAppRepository)
        {
            _lingoResourcesRepository = lingoResourcesRepository;
            _lingoAppRepository = lingoAppRepository;

            RuleFor(x => x.AppId).NotNull().NotEmpty();
            RuleFor(x => x.AppId).MustAsync(BeAnExistingLingoApp).WithMessage("LingoApp Id doesn't exist.").When(x => !x.AppId.IsNullOrBlank());

            RuleFor(x => x.Format).NotNull().NotEmpty();
            RuleFor(x => x.Format).Must(x => x == "json").WithMessage("Format not supported.").When(x => !x.Format.IsNullOrBlank());

            RuleFor(x => x.LanguageCode).NotNull().NotEmpty();
            RuleFor(x => x.LanguageCode).MustAsync(BeAnExistingLanguage).WithMessage("Language code doesn't exist.").When(x => !x.LanguageCode.IsNullOrBlank());
        }

        private async Task<bool> BeAnExistingLingoApp(string appId, CancellationToken token)
        {
            return await _lingoAppRepository.BeAnExistingLingoAppById(appId);
        }

        private async Task<bool> BeAnExistingLanguage(string languageCode, CancellationToken token)
        {
            return await _lingoResourcesRepository.BeAnExistingLanguage(languageCode);
        }
    }
}
