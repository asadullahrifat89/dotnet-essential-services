using Teams.ContentMangement.Domain.Entities;

namespace Teams.ContentMangement.Application.DTOs.Responses
{
    public class ProductRecommendation
    {
        /// Name of the product.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Details description of the product. Escaped rich text is expected here.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Amount of manpower available for this product.
        /// </summary>
        public int ManPower { get; set; } = 0;

        /// <summary>
        /// Amount of experience the manpower has for working on this product in years.
        /// </summary>
        public int Experience { get; set; } = 0;

        /// <summary>
        /// Empolyment types available for this product
        /// </summary>
        public EmploymentType[] EmploymentTypes { get; set; } = new EmploymentType[] { };

        /// <summary>
        /// Cost indication for this product.
        /// </summary>
        public ProductCostType ProductCostType { get; set; }

        /// <summary>
        /// Icon image url of this product.
        /// </summary>
        public string IconUrl { get; set; } = string.Empty;

        /// <summary>
        /// Match count against the submitted product search criterias.
        /// </summary>
        public int MatchCount { get; set; } = 0;

        /// <summary>
        /// Matching percentage against the submitted product search criteria count.
        /// </summary>
        public string MatchPercentage { get; set; } = string.Empty;

        public static ProductRecommendation Map(
            (Product product, int MatchCount) matchingProduct,
            int submittedProductSearchCriteriaCount)
        {
            return new ProductRecommendation()
            {
                Name = matchingProduct.product.Name,
                Description = matchingProduct.product.Description,
                Experience = matchingProduct.product.Experience,
                ManPower = matchingProduct.product.ManPower,
                EmploymentTypes = matchingProduct.product.EmploymentTypes,
                ProductCostType = matchingProduct.product.ProductCostType,
                IconUrl = matchingProduct.product.IconUrl,
                MatchCount = matchingProduct.MatchCount,
                MatchPercentage = submittedProductSearchCriteriaCount == 0 ? "0 %" : ((matchingProduct.MatchCount / submittedProductSearchCriteriaCount) * 100).ToString() + " %",
            };
        }
    }
}
