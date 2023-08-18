using Base.Application.DTOs.Responses;
using Identity.Application.Extensions;
using Identity.Domain.Entities;
using MediatR;
using Teams.ContentMangement.Domain.Entities;

namespace Teams.ContentMangement.Application.Commands
{
    public class AddProductCommand : IRequest<ServiceResponse>
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public int ManPower { get; set; } = 0;

        public int Experience { get; set; } = 0;

        public EmploymentType[] EmploymentTypes { get; set; } = new EmploymentType[] { };

        public ProductCostType ProductCostType { get; set; }

        public string IconUrl { get; set; } = string.Empty;

        public string[] LinkedProductSearchCriteriaIds { get; set; } = new string[0];

        public static Product Initialize(AddProductCommand command, AuthenticationContext authenticationContext)
        {
            var product = new Product()
            {
                Name = command.Name,
                Description = command.Description,
                ManPower = command.ManPower,
                Experience = command.Experience,
                EmploymentTypes = command.EmploymentTypes,
                ProductCostType = command.ProductCostType,
                IconUrl = command.IconUrl,
                PublishingStatus = PublishingStatus.Published,
                TimeStamp = authenticationContext.BuildCreatedByTimeStamp(),
            };

            return product;
        }

    }
}
