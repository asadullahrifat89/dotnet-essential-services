using MongoDB.Bson.Serialization.Attributes;

namespace BaseModule.Models.Entities
{
    [BsonIgnoreExtraElements]
    public class Tenant
    {
        [BsonId]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; } = string.Empty;
    }
}
