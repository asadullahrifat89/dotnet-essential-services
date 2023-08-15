using FluentValidation;
using TeamsCore.Declarations.Commands;
using TeamsCore.Declarations.Repositories;
using TeamsCore.Models.Entities;

namespace TeamsCore.Implementations.Commands.Validators
{
    public class AddProductCommandValidator : AbstractValidator<AddProductCommand>
    {
        private readonly ISearchCriteriaRepository _searchCriteriaRepository;
        public AddProductCommandValidator(ISearchCriteriaRepository searchCriteriaRepository)
        {
            _searchCriteriaRepository = searchCriteriaRepository;

            RuleFor(x => x.Name).NotNull().NotEmpty().WithMessage("Name must not be empty");
            RuleFor(x => x.Description).NotNull().NotEmpty().WithMessage("Description must not be empty");
            RuleFor(x => x.ManPower).NotNull().NotEmpty().WithMessage("ManPower must not be empty");
            RuleFor(x => x.Experience).NotNull().NotEmpty().WithMessage("Experience must not be empty");
            RuleFor(x => x.EmploymentType).NotNull().NotEmpty().WithMessage("EmploymentType must not be empty");
            RuleFor(x => x.ProductCostType).NotNull().NotEmpty().WithMessage("ProductCostType must not be empty");
            RuleFor(x => x.LinkedSearchCriteriaIds).NotEmpty().WithMessage("LinkedProductIds is required.");

            RuleFor(x => x.ProductCostType).Must(x => x == ProductCostType.Low || x == ProductCostType.Medium || x == ProductCostType.High).WithMessage("Invalid ProductCost type.").When(x => x.ProductCostType != null);

            RuleFor(x => x).MustAsync(BeAnExistingSearchCriteriaId).WithMessage("Search Criteria Id doesn't exist.").When(x => x.LinkedSearchCriteriaIds != null);
        }

        private async Task<bool> BeAnExistingSearchCriteriaId(AddProductCommand command, CancellationToken arg2)
        {
            
            foreach (var searchCriteriaId in command.LinkedSearchCriteriaIds)
            {
                var exists = await _searchCriteriaRepository.BeAnExistingSearchCriteriaById(searchCriteriaId);

                if (!exists)
                   return false;
            }

            return true;
        }
    }
}
