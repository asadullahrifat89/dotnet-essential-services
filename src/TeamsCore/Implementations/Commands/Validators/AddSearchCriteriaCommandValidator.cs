using BaseCore.Extensions;
using FluentValidation;
using TeamsCore.Declarations.Commands;
using TeamsCore.Declarations.Repositories;
using TeamsCore.Models.Entities;

namespace TeamsCore.Implementations.Commands.Validators
{
    public class AddSearchCriteriaCommandValidator : AbstractValidator<AddSearchCriteriaCommand>
    {
        private readonly ISearchCriteriaRepository _searchCriteriaRepository;

        public AddSearchCriteriaCommandValidator(ISearchCriteriaRepository searchCriteriaRepository)
        {
            _searchCriteriaRepository = searchCriteriaRepository;

            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Name must not be empty");
            RuleFor(x => x).MustAsync(NotBeAnExistingSearchCriteria).WithMessage("Name already exists.").When(x => !x.Name.IsNullOrBlank());

            RuleFor(x => x.Description).NotNull().NotEmpty().WithMessage("Description must not be empty");

            RuleFor(x => x.IconUrl).NotNull().NotEmpty().WithMessage("IconUrl must not be empty");

            RuleFor(x => x.SearchCriteriaType).NotNull().NotEmpty().IsInEnum().WithMessage("SearchCriteria Type is not acceptable");

            RuleFor(x => x.SkillsetType).NotNull().NotEmpty().IsInEnum().WithMessage("Skillset Type is not acceptable");
        }

        private async Task<bool> NotBeAnExistingSearchCriteria(AddSearchCriteriaCommand command, CancellationToken token)
        {
            return !await _searchCriteriaRepository.BeAnExistingSearchCriteria(searchCriteria: command.Name);
        }
    }
}
