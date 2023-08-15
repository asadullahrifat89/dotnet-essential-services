using BaseCore.Extensions;
using BaseCore.Models.Entities;
using System.Text.Json.Serialization;
using TeamsCore.Declarations.Commands;

namespace TeamsCore.Models.Entities
{
    public class Product : EntityBase
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ManPower { get; set; } = string.Empty;

        public string Experience { get; set; } = string.Empty;

        public string EmploymentType { get; set; } = string.Empty;

        public ProductCostType ProductCostType { get; set; }

        public string IconUrl { get; set; } = string.Empty;

        public string BannerUrl { get; set; } = string.Empty;


        public static Product Initialize(AddProductCommand command, AuthenticationContext authenticationContext)
        {
            var product = new Product()
            {
                Name = command.Name,
                Description = command.Description,
                ManPower = command.ManPower,
                Experience = command.Experience,
                EmploymentType = command.EmploymentType,
                ProductCostType = command.ProductCostType,
                IconUrl = command.IconUrl,
                BannerUrl = command.BannerUrl,
                TimeStamp = authenticationContext.BuildCreatedByTimeStamp(),
            };

            return product;
        }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ProductCostType
    {
        Low,
        Medium,
        High
    }
 
}
