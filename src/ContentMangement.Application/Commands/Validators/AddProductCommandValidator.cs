using FluentValidation;
using Teams.ContentMangement.Domain.Repositories.Interfaces;

namespace Teams.ContentMangement.Application.Commands.Validators
{
    public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
    {
        private readonly IProductSearchCriteriaRepository _searchCriteriaRepository;
        public AddProductCommandValidator(IProductSearchCriteriaRepository searchCriteriaRepository)
        {
            _searchCriteriaRepository = searchCriteriaRepository;

            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Name must not be empty");
            RuleFor(x => x.Description).NotNull().NotEmpty().WithMessage("Description must not be empty");
            RuleFor(x => x.ManPower).NotNull().NotEmpty().WithMessage("ManPower must not be empty");
            RuleFor(x => x.Experience).NotNull().NotEmpty().WithMessage("Experience must not be empty");
            RuleFor(x => x.EmploymentTypes).NotNull().NotEmpty().WithMessage("EmploymentTypes must not be empty");
            RuleFor(x => x.ProductCostType).NotNull().NotEmpty().WithMessage("ProductCostType must not be empty");
            RuleFor(x => x.LinkedProductSearchCriteriaIds).NotNull().NotEmpty().WithMessage("Linked Search Criteria Ids required.");
            RuleFor(x => x.ProductCostType).IsInEnum().WithMessage("Invalid ProductCost type.");
            RuleFor(x => x).MustAsync(BeAnExistingSearchCriteriaId).WithMessage("Product Search Criteria doesn't exist.").When(x => x.LinkedProductSearchCriteriaIds != null);
        }

        private async Task<bool> BeAnExistingSearchCriteriaId(AddProductCommand command, CancellationToken arg2)
        {
            foreach (var id in command.LinkedProductSearchCriteriaIds)
            {
                var exists = await _searchCriteriaRepository.BeAnExistingProductSearchCriteriaById(id);

                if (!exists)
                    return false;
            }

            return true;
        }
    }
}
