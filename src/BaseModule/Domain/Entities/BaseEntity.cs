using MongoDB.Bson.Serialization.Attributes;

namespace BaseModule.Domain.Entities
{
    [BsonIgnoreExtraElements]
    public class BaseEntity
    {
        [BsonId]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public TimeStamp TimeStamp { get; set; } = new TimeStamp();

        //public string TenantId { get; set; } = string.Empty;
    }
}
