using BaseModule.Infrastructure.Extensions;
using FluentValidation;
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

            RuleFor(x => x.ProductSearchCriteriaType).NotNull().NotEmpty().IsInEnum().WithMessage("ProductSearchCriteria Type is not acceptable.");
            RuleFor(x => x.SkillsetType).NotNull().NotEmpty().IsInEnum().WithMessage("Skillset Type is not acceptable.");
        }

        private async Task<bool> BeAnExistingProductSearchCriteriaById(string productSearchCriteriaId, CancellationToken token)
        {
            return await _productSearchCriteriaRepository.BeAnExistingProductSearchCriteriaById(productSearchCriteriaId);
        }
    }
}
