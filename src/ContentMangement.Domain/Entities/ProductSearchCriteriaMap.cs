using MongoDB.Bson.Serialization.Attributes;

namespace Teams.ContentMangement.Domain.Entities
{
    [BsonIgnoreExtraElements]
    public class ProductSearchCriteriaMap
    {
        [BsonId]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        /// <summary>
        /// Id of the mapped product.
        /// </summary>
        public string ProductId { get; set; } = string.Empty;

        /// <summary>
        /// Id of the mapped product search criteria.
        /// </summary>
        public string ProductSearchCriteriaId { get; set; } = string.Empty;
    }
}
