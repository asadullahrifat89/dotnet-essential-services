using BaseModule.Infrastructure.Extensions;
using FluentValidation;
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

            RuleFor(x => x.ProductSearchCriteriaType).NotNull().NotEmpty().IsInEnum().WithMessage("ProductSearchCriteria Type is not acceptable.");

            RuleFor(x => x.SkillsetType).NotNull().NotEmpty().IsInEnum().WithMessage("Skillset Type is not acceptable.");
        }

        private async Task<bool> NotBeAnExistingProductSearchCriteria(AddProductSearchCriteriaCommand command, CancellationToken token)
        {
            return !await _productSearchCriteriaRepository.BeAnExistingProductSearchCriteria(name: command.Name);
        }
    }
}
