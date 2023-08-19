using Base.Application.DTOs.Responses;
using Identity.Application.Extensions;
using Identity.Domain.Entities;
using MediatR;
using Teams.ContentMangement.Domain.Entities;

namespace Teams.ContentMangement.Application.Commands
{
    public class AddProductSearchCriteriaCommand : IRequest<ServiceResponse>
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string IconUrl { get; set; } = string.Empty;

        //public SearchCriteriaType SearchCriteriaType { get; set; }

        public SkillsetType SkillsetType { get; set; }

        public static ProductSearchCriteria Map(AddProductSearchCriteriaCommand command, AuthenticationContext authCtx)
        {
            var searchCriteria = new ProductSearchCriteria()
            {
                Name = command.Name,
                Description = command.Description,
                IconUrl = command.IconUrl,
                //SearchCriteriaType = command.SearchCriteriaType,
                SkillsetType = command.SkillsetType,
                TimeStamp = authCtx.BuildCreatedByTimeStamp(),
            };

            return searchCriteria;
        }
    }
}
