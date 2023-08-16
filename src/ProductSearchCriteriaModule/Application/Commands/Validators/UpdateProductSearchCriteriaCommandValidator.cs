using BaseModule.Infrastructure.Extensions;
using FluentValidation;
using ProductSearchCriteriaModule.Domain.Entities;
using ProductSearchCriteriaModule.Domain.Repositories.Interfaces;

namespace ProductSearchCriteriaModule.Application.Commands.Validators
{
    public class UpdateProductSearchCriteriaCommandValidator : AbstractValidator<UpdateProductSearchCriteriaCommand>
    {
        private readonly IProductSearchCriteriaRepository _productSearchCriteriaRepository;

        public UpdateProductSearchCriteriaCommandValidator(IProductSearchCriteriaRepository ProductSearchCriteriaRepository)
        {
            _productSearchCriteriaRepository = ProductSearchCriteriaRepository;

            RuleFor(x => x.ProductSearchCriteriaId).NotNull().NotEmpty().WithMessage("Id must not be empty.");
            RuleFor(x => x.ProductSearchCriteriaId).MustAsync(BeAnExistingProductSearchCriteriaById).WithMessage("ProductSearchCriteria does not exist.").When(x => !x.ProductSearchCriteriaId.IsNullOrBlank());

            RuleFor(x => x.ProductSearchCriteriaType).Must(x => x == ProductSearchCriteriaType.Discipline || x == ProductSearchCriteriaType.Skillset).WithMessage("ProductSearchCriteria Type is not acceptable.").When(x => x.ProductSearchCriteriaType != null);
            RuleFor(x => x.SkillsetType).Must(x => x == SkillsetType.Soft || x == SkillsetType.Hard || x == SkillsetType.Generic).WithMessage("Skillset Type is not acceptable.").When(x => x.SkillsetType != null);

        }

        private async Task<bool> BeAnExistingProductSearchCriteriaById(string productSearchCriteriaId, CancellationToken token)
        {
            return await _productSearchCriteriaRepository.BeAnExistingProductSearchCriteriaById(productSearchCriteriaId);
        }
    }
}
