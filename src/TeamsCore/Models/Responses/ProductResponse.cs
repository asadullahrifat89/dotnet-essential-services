using TeamsCore.Models.Entities;

namespace TeamsCore.Models.Responses
{
    public class ProductResponse : Product
    {
        public AttachedSearchCriteria[] AttachedSearchCriterias { get; set; } = new AttachedSearchCriteria[0];

        public AttachedProject[] AttachedProjects { get; set; } = new AttachedProject[0];

        public static ProductResponse Initialize(Product product)
        {
            var productResponse = new ProductResponse()
            {
                Id = product.Id,
                Name = product.Name,
                Description = product.Description,
                ManPower = product.ManPower,
                Experience = product.Experience,
                EmploymentType = product.EmploymentType,
                ProductCostType = product.ProductCostType,
                IconUrl = product.IconUrl,
                BannerUrl = product.BannerUrl,
                TimeStamp = product.TimeStamp,
            };

            // search in ProductSearchCriteriaMap and make AttachedSearchCriteria[] AttachedSearchCriterias
            return productResponse;
        }
    }

    public class AttachedSearchCriteria
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string IconUrl { get; set; } = string.Empty;
    }

    public class AttachedProject : AttachedSearchCriteria
    {
        public string Description { get; set; } = string.Empty;

        public string Link { get; set; } = string.Empty;
    }
}
