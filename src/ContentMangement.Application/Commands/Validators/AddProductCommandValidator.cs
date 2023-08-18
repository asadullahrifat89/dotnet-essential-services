using FluentValidation;
using Teams.ContentMangement.Domain.Repositories.Interfaces;

namespace Teams.ContentMangement.Application.Commands.Validators
{
    public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
    {
        private readonly IProductSearchCriteriaRepository _productSearchCriteriaRepository;

        public AddProductCommandValidator(IProductSearchCriteriaRepository searchCriteriaRepository)
        {
            _productSearchCriteriaRepository = searchCriteriaRepository;

            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Name must not be empty");
            RuleFor(x => x.Description).NotNull().NotEmpty().WithMessage("Description must not be empty");
            RuleFor(x => x.ManPower).GreaterThan(0).WithMessage("ManPower must be non zero.");
            RuleFor(x => x.Experience).GreaterThan(0).WithMessage("Experience must be non zero.");
            RuleFor(x => x.EmploymentTypes).NotNull().NotEmpty().WithMessage("EmploymentTypes must not be empty");            
            RuleFor(x => x.ProductCostType).IsInEnum().WithMessage("Invalid ProductCost type.");
            RuleFor(x => x.LinkedProductSearchCriteriaIds).NotNull().NotEmpty().WithMessage("Linked Search Criteria Ids required.");
            RuleFor(x => x.LinkedProductSearchCriteriaIds).Must(x => x.Length < 11).WithMessage("Can not attach more than 10 search criterias.").When(x => x.LinkedProductSearchCriteriaIds is not null && x.LinkedProductSearchCriteriaIds.Any());            
            RuleFor(x => x).MustAsync(BeAnExistingSearchCriteriaId).WithMessage("Product Search Criteria doesn't exist.").When(x => x.LinkedProductSearchCriteriaIds != null);
        }

        private async Task<bool> BeAnExistingSearchCriteriaId(AddProductCommand command, CancellationToken arg2)
        {
            foreach (var id in command.LinkedProductSearchCriteriaIds)
            {
                var exists = await _productSearchCriteriaRepository.BeAnExistingProductSearchCriteriaById(id);

                if (!exists)
                    return false;
            }

            return true;
        }
    }
}
