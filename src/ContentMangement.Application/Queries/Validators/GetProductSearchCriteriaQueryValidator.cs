using Base.Application.Extensions;
using FluentValidation;
using Teams.ContentMangement.Domain.Repositories.Interfaces;

namespace Teams.ContentMangement.Application.Queries.Validators
{
    public class GetProductSearchCriteriaQueryValidator : AbstractValidator<GetProductSearchCriteriaQuery>
    {
        private readonly IProductSearchCriteriaRepository _productSearchCriteriaRepository;

        public GetProductSearchCriteriaQueryValidator(IProductSearchCriteriaRepository ProductSearchCriteriaRepository)
        {
            _productSearchCriteriaRepository = ProductSearchCriteriaRepository;

            RuleFor(x => x.ProductSearchCriteriaId).NotNull().NotEmpty().WithMessage("ProductSearchCriteria Id must not be empty");
            RuleFor(x => x).MustAsync(BeAnExistingProductSearchCriteriaById).WithMessage("ProductSearchCriteria does not exist.").When(x => !x.ProductSearchCriteriaId.IsNullOrBlank());
        }

        private async Task<bool> BeAnExistingProductSearchCriteriaById(GetProductSearchCriteriaQuery query, CancellationToken token)
        {
            return await _productSearchCriteriaRepository.BeAnExistingProductSearchCriteriaById(id: query.ProductSearchCriteriaId);
        }
    }
}
