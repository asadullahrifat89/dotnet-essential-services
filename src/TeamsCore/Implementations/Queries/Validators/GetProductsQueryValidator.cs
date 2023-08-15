using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsCore.Declarations.Queries;
using TeamsCore.Models.Entities;

namespace TeamsCore.Implementations.Queries.Validators
{
    public class GetProductsQueryValidator : AbstractValidator<GetProductsQuery>
    {
        public GetProductsQueryValidator()
        {
            RuleFor(x => x.PageIndex).GreaterThanOrEqualTo(0);

            RuleFor(x => x.PageSize).GreaterThan(0);

            RuleFor(x => x.SearchTerm).NotNull().NotEmpty();

            RuleFor(x => x.ProductCostType)
                .Must(BeAnExistingProductCostType)
                .When(x => x.ProductCostType.HasValue)
                .WithMessage("Invalid ProductCostType");
        }

        private bool BeAnExistingProductCostType(ProductCostType? nullable)
        {
            return Enum.IsDefined(typeof(ProductCostType), nullable);
        }
    }
}
