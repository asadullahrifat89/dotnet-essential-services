using Base.Application.Extensions;
using ContentMangement.Domain.Repositories.Interfaces;
using FluentValidation;

namespace ContentMangement.Application.Commands.Validators
{
    public class AddProductSearchCriteriaCommandValidator : AbstractValidator<AddProductSearchCriteriaCommand>
    {
        private readonly IProductSearchCriteriaRepository _searchCriteriaRepository;

        public AddProductSearchCriteriaCommandValidator(IProductSearchCriteriaRepository searchCriteriaRepository)
        {
            _searchCriteriaRepository = searchCriteriaRepository;

            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Name must not be empty");
            RuleFor(x => x).MustAsync(NotBeAnExistingSearchCriteria).WithMessage("Name already exists.").When(x => !x.Name.IsNullOrBlank());

            RuleFor(x => x.Description).NotNull().NotEmpty().WithMessage("Description must not be empty");

            RuleFor(x => x.IconUrl).NotNull().NotEmpty().WithMessage("IconUrl must not be empty");

            RuleFor(x => x.SearchCriteriaType).NotNull().NotEmpty().IsInEnum().WithMessage("SearchCriteria Type is not acceptable");

            RuleFor(x => x.SkillsetType).NotNull().NotEmpty().IsInEnum().WithMessage("Skillset Type is not acceptable");
        }

        private async Task<bool> NotBeAnExistingSearchCriteria(AddProductSearchCriteriaCommand command, CancellationToken token)
        {
            return !await _searchCriteriaRepository.BeAnExistingProductSearchCriteria(name: command.Name);
        }
    }
}
