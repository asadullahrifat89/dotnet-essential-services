using MongoDB.Bson.Serialization.Attributes;

namespace Base.Domain.Entities
{
    [BsonIgnoreExtraElements]
    public class EntityBase
    {
        [BsonId]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public TimeStamp TimeStamp { get; set; } = new TimeStamp();

        //public string TenantId { get; set; } = string.Empty;
    }
}
