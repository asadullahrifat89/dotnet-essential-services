using BaseModule.Infrastructure.Extensions;
using FluentValidation;
using LanguageModule.Declarations.Queries;
using LanguageModule.Declarations.Repositories;

namespace LanguageModule.Implementations.Queries.Validators
{
    public class GetLingoAppQueryValidator : AbstractValidator<GetLingoAppQuery>
    {
        private readonly ILingoAppRepository _lingoAppRepository;

        public GetLingoAppQueryValidator(ILingoAppRepository lingoAppRepository)
        {
            _lingoAppRepository = lingoAppRepository;

            RuleFor(x => x.AppId).NotNull().NotEmpty();
            RuleFor(x => x.AppId).MustAsync(BeAnExistingLingoApp).WithMessage("Lingo AppId doesn't exist.").When(x => !x.AppId.IsNullOrBlank());
        }

        private Task<bool> BeAnExistingLingoApp(string appId, CancellationToken token)
        {
            return _lingoAppRepository.BeAnExistingLingoAppById(appId);
        }
    }
}
