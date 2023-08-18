using Identity.Domain.Entities;
using Teams.ContentMangement.Domain.Entities;

namespace Teams.ContentMangement.Application.Commands
{
    public class UpdateProductCommand : AddProductCommand
    {
        public string ProductId { get; set; } = string.Empty;

        public PublishingStatus PublishingStatus { get; set; } = PublishingStatus.Published;

        public static Product Initialize(UpdateProductCommand command, AuthenticationContext authenticationContext)
        {
            var product = new Product()
            {
                Id = command.ProductId,
                Name = command.Name,
                Description = command.Description,
                ManPower = command.ManPower,
                Experience = command.Experience,
                EmploymentTypes = command.EmploymentTypes,
                ProductCostType = command.ProductCostType,
                IconUrl = command.IconUrl,
                PublishingStatus = command.PublishingStatus,
            };

            return product;
        }
    }
}
