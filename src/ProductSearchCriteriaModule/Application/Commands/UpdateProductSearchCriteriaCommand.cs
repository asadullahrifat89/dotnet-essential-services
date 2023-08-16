using IdentityModule.Domain.Entities;
using ProductSearchCriteriaModule.Domain.Entities;

namespace ProductSearchCriteriaModule.Application.Commands
{
    public class UpdateProductSearchCriteriaCommand : AddProductSearchCriteriaCommand
    {
        public string ProductSearchCriteriaId { get; set; } = string.Empty;

        public static ProductSearchCriteria Initialize(UpdateProductSearchCriteriaCommand command, AuthenticationContext authCtx)
        {
            var searchCriteria = new ProductSearchCriteria()
            {
                Id = command.ProductSearchCriteriaId,
                Name = command.Name,
                Description = command.Description,
                IconUrl = command.IconUrl,
                SearchCriteriaType = command.ProductSearchCriteriaType,
                SkillsetType = command.SkillsetType,
            };

            return searchCriteria;
        }
    }
}
