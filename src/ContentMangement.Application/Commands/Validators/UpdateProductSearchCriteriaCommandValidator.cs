using Base.Application.Extensions;
using FluentValidation;
using Teams.ContentMangement.Domain.Repositories.Interfaces;

namespace Teams.ContentMangement.Application.Commands.Validators
{
    public class UpdateProductSearchCriteriaCommandValidator : AbstractValidator<UpdateProductSearchCriteriaCommand>
    {
        private readonly IProductSearchCriteriaRepository _searchCriteriaRepository;

        public UpdateProductSearchCriteriaCommandValidator(IProductSearchCriteriaRepository searchCriteriaRepository)
        {
            _searchCriteriaRepository = searchCriteriaRepository;

            RuleFor(x => x.SearchCriteriaId).NotNull().NotEmpty().WithMessage("Id must not be empty");
            RuleFor(x => x.SearchCriteriaId).MustAsync(BeAnExistingSearchCriteriaById).WithMessage("SearchCriteria does not exist.").When(x => !x.SearchCriteriaId.IsNullOrBlank());

            RuleFor(x => x.SearchCriteriaType).NotNull().NotEmpty().IsInEnum().WithMessage("SearchCriteria Type is not acceptable");

            RuleFor(x => x.SkillsetType).NotNull().NotEmpty().IsInEnum().WithMessage("Skillset Type is not acceptable");
        }

        private async Task<bool> BeAnExistingSearchCriteriaById(string searchCriteriaId, CancellationToken token)
        {
            return await _searchCriteriaRepository.BeAnExistingProductSearchCriteriaById(searchCriteriaId);
        }
    }
}
