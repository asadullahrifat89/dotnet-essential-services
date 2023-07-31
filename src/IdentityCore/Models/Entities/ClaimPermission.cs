using MongoDB.Bson.Serialization.Attributes;

namespace IdentityCore.Models.Entities
{
    [BsonIgnoreExtraElements]
    public class ClaimPermission
    {

        [BsonId]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; } = string.Empty;
    }
}
