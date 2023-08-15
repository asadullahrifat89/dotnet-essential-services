using BaseModule.Application.DTOs.Responses;
using IdentityModule.Domain.Entities;
using MediatR;
using IdentityModule.Infrastructure.Extensions;
using ProductSearchCriteriaModule.Domain.Entities;

namespace ProductSearchCriteriaModule.Application.Commands
{
    public class AddProductSearchCriteriaCommand : IRequest<ServiceResponse>
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string IconUrl { get; set; } = string.Empty;

        public ProductSearchCriteriaType SearchCriteriaType { get; set; }

        public SkillsetType SkillsetType { get; set; }

        public static ProductSearchCriteria Initialize(AddProductSearchCriteriaCommand command, AuthenticationContext authCtx)
        {
            var searchCriteria = new ProductSearchCriteria()
            {
                Name = command.Name,
                Description = command.Description,
                IconUrl = command.IconUrl,
                SearchCriteriaType = command.SearchCriteriaType,
                SkillsetType = command.SkillsetType,
                TimeStamp = authCtx.BuildCreatedByTimeStamp(),
            };

            return searchCriteria;
        }
    }
}
