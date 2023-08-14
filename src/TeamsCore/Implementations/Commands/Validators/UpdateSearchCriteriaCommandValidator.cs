using BaseCore.Extensions;
using FluentValidation;
using TeamsCore.Declarations.Commands;
using TeamsCore.Declarations.Repositories;

namespace TeamsCore.Implementations.Commands.Validators
{
    public class UpdateSearchCriteriaCommandValidator : AbstractValidator<UpdateSearchCriteriaCommand>
    {
        private readonly ISearchCriteriaRepository _searchCriteriaRepository;

        public UpdateSearchCriteriaCommandValidator(ISearchCriteriaRepository searchCriteriaRepository)
        {
            _searchCriteriaRepository = searchCriteriaRepository;

            RuleFor(x => x.Id).NotNull().NotEmpty().WithMessage("Id must not be empty");
            RuleFor(x => x.Id).MustAsync(BeAnExistingSearchCriteriaById).WithMessage("SearchCriteria does not exist.").When(x => !x.Id.IsNullOrBlank());

            RuleFor(x => x.SearchCriteriaType).NotNull().NotEmpty().IsInEnum().WithMessage("SearchCriteria Type is not acceptable");

            RuleFor(x => x.SkillsetType).NotNull().NotEmpty().IsInEnum().WithMessage("Skillset Type is not acceptable");
        }

        private async Task<bool> BeAnExistingSearchCriteriaById(string searchCriteriaId, CancellationToken token)
        {
            return await _searchCriteriaRepository.BeAnExistingSearchCriteriaById(searchCriteriaId);
        }
    }
}
