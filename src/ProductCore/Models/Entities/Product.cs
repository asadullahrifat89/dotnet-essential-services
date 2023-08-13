using BaseCore.Models.Entities;

namespace ProductCore.Models.Entities
{
    public class Product : EntityBase
    {
        public string ProductName { get; set; } = string.Empty;
        public int ProductCost { get; set; } = 0;
        public string ProductDescription { get; set; } = string.Empty;
        public string ProductLogoName { get; set; } = string.Empty;
        public ProductType ProductType { get; set; } = new ProductType();
        public UpperLowerBound ManPower { get; set; } = new UpperLowerBound();
        public UpperLowerBound Experience { get; set; } = new UpperLowerBound();

        // Attachted Search Criteria Reference
    }

    public class ProductType
    {
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
    }

    public class UpperLowerBound
    {
        public int Max { get; set; } = 0;
        public int Min { get; set; } = 0;
    }
}