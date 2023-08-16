using BaseModule.Infrastructure.Extensions;
using FluentValidation;
using ProductSearchCriteriaModule.Domain.Entities;
using ProductSearchCriteriaModule.Domain.Repositories.Interfaces;

namespace ProductSearchCriteriaModule.Application.Commands.Validators
{
    public class AddProductSearchCriteriaCommandValidator : AbstractValidator<AddProductSearchCriteriaCommand>
    {
        private readonly IProductSearchCriteriaRepository _productSearchCriteriaRepository;

        public AddProductSearchCriteriaCommandValidator(IProductSearchCriteriaRepository ProductSearchCriteriaRepository)
        {
            _productSearchCriteriaRepository = ProductSearchCriteriaRepository;

            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Name must not be empty");
            RuleFor(x => x).MustAsync(NotBeAnExistingProductSearchCriteria).WithMessage("Name already exists.").When(x => !x.Name.IsNullOrBlank());

            RuleFor(x => x.Description).NotNull().NotEmpty().WithMessage("Description must not be empty.");

            RuleFor(x => x.IconUrl).NotNull().NotEmpty().WithMessage("IconUrl must not be empty.");

            RuleFor(x => x.ProductSearchCriteriaType).Must(x => x == ProductSearchCriteriaType.Discipline || x == ProductSearchCriteriaType.Skillset).WithMessage("ProductSearchCriteria Type is not acceptable.").When(x => x.ProductSearchCriteriaType != null);

            RuleFor(x => x.SkillsetType).Must(x => x == SkillsetType.Soft || x == SkillsetType.Hard || x == SkillsetType.Generic).WithMessage("Skillset Type is not acceptable.").When(x => x.SkillsetType != null);



        }

        private async Task<bool> NotBeAnExistingProductSearchCriteria(AddProductSearchCriteriaCommand command, CancellationToken token)
        {
            return !await _productSearchCriteriaRepository.BeAnExistingProductSearchCriteriaByName(name: command.Name);
        }
    }
}
