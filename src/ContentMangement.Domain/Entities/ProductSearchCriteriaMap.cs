using MongoDB.Bson.Serialization.Attributes;

namespace SharedModule.Domain.Entities
{
    [BsonIgnoreExtraElements]
    public class ProductSearchCriteriaMap
    {
        [BsonId]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string ProductId { get; set; } = string.Empty;

        public string ProductSearchCriteriaId { get; set; } = string.Empty;
    }
}
