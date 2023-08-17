using MongoDB.Bson.Serialization.Attributes;

namespace Identity.Domain.Entities
{
    [BsonIgnoreExtraElements]
    public class UserRoleMap
    {
        [BsonId]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string UserId { get; set; } = string.Empty;

        public string RoleId { get; set; } = string.Empty;
    }
}
