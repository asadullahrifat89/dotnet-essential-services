using BaseCore.Extensions;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsCore.Declarations.Commands;
using TeamsCore.Declarations.Repositories;

namespace TeamsCore.Implementations.Commands.Validators
{
    public class UpdateProductCommandValidator : AbstractValidator<UpdateProductCommand>
    {
        private readonly IProductRepository _productRepository;
        private readonly ISearchCriteriaRepository _searchCriteriaRepository;
        
        public UpdateProductCommandValidator(IProductRepository productRepository, ISearchCriteriaRepository searchCriteriaRepository)
        {
            _productRepository = productRepository;
            
            RuleFor(x => x.ProductId).NotNull().NotEmpty();
            RuleFor(x => x.ProductId)
                .MustAsync(BeAnExistingProductId)
                .WithMessage("Product does not exist").When(x => !x.ProductId.IsNullOrBlank());

            RuleFor(x => x.Name).NotEmpty().NotNull();
            RuleFor(x => x.Description).NotNull().NotEmpty();
            RuleFor(x => x.ManPower).NotNull().NotEmpty();
            RuleFor(x => x.EmploymentType).NotNull().NotEmpty();
            RuleFor(x => x.IconUrl).NotNull().NotEmpty();
            RuleFor(x => x.Experience).NotNull().NotEmpty();
            RuleFor(x => x.BannerUrl).NotNull().NotEmpty();
            RuleFor(x => x.ProductCostType).NotNull().NotEmpty().IsInEnum();
           
            RuleFor(x => x.LinkedSearchCriteriaIds)
                .MustAsync(BeAnExistingSearchCriteriaId)
                .WithMessage("Search criteria does not exist");
        }

        private async Task<bool> BeAnExistingSearchCriteriaId(string[] LinkedSearchCriteriaIds, CancellationToken token)
        {
            foreach (var searchCriteriaId in LinkedSearchCriteriaIds)
            {
                var exists = await _searchCriteriaRepository.BeAnExistingSearchCriteriaById(searchCriteriaId);

                if (!exists)
                    return false;
            }

            return true;
        }

        private async Task<bool> BeAnExistingProductId(string productId, CancellationToken token)
        {
            return await _productRepository.BeAnExistingProductId(productId);
        }
    }
}
