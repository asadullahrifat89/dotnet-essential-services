using MongoDB.Bson.Serialization.Attributes;

namespace BaseModule.Domain.Entities
{
    [BsonIgnoreExtraElements]
    public class Tenant
    {
        [BsonId]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; } = string.Empty;
    }
}
