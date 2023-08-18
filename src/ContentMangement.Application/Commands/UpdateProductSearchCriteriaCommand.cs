using Identity.Domain.Entities;
using Teams.ContentMangement.Domain.Entities;

namespace Teams.ContentMangement.Application.Commands
{
    public class UpdateProductSearchCriteriaCommand : AddProductSearchCriteriaCommand
    {
        public string SearchCriteriaId { get; set; } = string.Empty;

        public static ProductSearchCriteria Initialize(UpdateProductSearchCriteriaCommand command, AuthenticationContext authCtx)
        {
            var searchCriteria = new ProductSearchCriteria()
            {
                Id = command.SearchCriteriaId,
                Name = command.Name,
                Description = command.Description,
                IconUrl = command.IconUrl,
                //SearchCriteriaType = command.SearchCriteriaType,
                SkillsetType = command.SkillsetType,
            };

            return searchCriteria;
        }
    }
}
