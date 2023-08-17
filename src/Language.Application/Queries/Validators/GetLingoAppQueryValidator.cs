using Base.Application.Extensions;
using FluentValidation;
using Language.Domain.Repositories.Interfaces;

namespace Language.Application.Queries.Validators
{
    public class GetLingoAppQueryValidator : AbstractValidator<GetLingoAppQuery>
    {
        private readonly ILanguageAppRepository _lingoAppRepository;

        public GetLingoAppQueryValidator(ILanguageAppRepository lingoAppRepository)
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
