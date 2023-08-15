using MongoDB.Bson.Serialization.Attributes;

namespace BaseModule.Models.Entities
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
