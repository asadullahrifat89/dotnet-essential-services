using FluentValidation;
using Teams.ContentMangement.Domain.Repositories.Interfaces;

namespace Teams.CustomerEngagement.Application.Commands.Validators
{
    public class AddQuotationCommandValidator : AbstractValidator<AddQuotationCommand>
    {
        private readonly IProductSearchCriteriaRepository _productSearchCriteriaRepository;

        public AddQuotationCommandValidator(IProductSearchCriteriaRepository productSearchCriteriaRepository)
        {
            _productSearchCriteriaRepository = productSearchCriteriaRepository;

            RuleFor(x => x.Email).NotNull().NotEmpty().WithMessage("Email must not be empty");
            RuleFor(x => x.Title).NotNull().NotEmpty().WithMessage("Title must not be empty");
            RuleFor(x => x.Location).NotNull().NotEmpty().WithMessage("Location must not be empty");
            RuleFor(x => x.ManPower).GreaterThan(0).WithMessage("ManPower must be non zero");
            RuleFor(x => x.Experience).GreaterThan(0).WithMessage("Experience must be non zero");
            RuleFor(x => x.EmploymentTypes).NotNull();
            RuleFor(x => x.EmploymentTypes).Must(x => x.Length > 0).WithMessage("At least one employment type is required.").When(x => x.EmploymentTypes is not null);
            RuleFor(x => x.SubmittedProductSearchCriterias).NotNull();
            RuleFor(x => x.SubmittedProductSearchCriterias).Must(x => x.Length > 0).WithMessage("At least one product search criteria is required.").When(x => x.SubmittedProductSearchCriterias is not null);
            RuleFor(x => x).MustAsync(BeAnExistingSearchCriteriaId).WithMessage("One or more product search critera doesn't exist.").When(x => x.SubmittedProductSearchCriterias is not null);
        }

        private async Task<bool> BeAnExistingSearchCriteriaId(AddQuotationCommand command, CancellationToken arg2)
        {
            foreach (var id in command.SubmittedProductSearchCriterias.Select(x => x.Id))
            {
                var exists = await _productSearchCriteriaRepository.BeAnExistingProductSearchCriteriaById(id);

                if (!exists)
                    return false;
            }

            return true;
        }
    }
}
