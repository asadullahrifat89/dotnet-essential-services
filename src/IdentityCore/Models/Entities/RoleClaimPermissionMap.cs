using MongoDB.Bson.Serialization.Attributes;

namespace IdentityCore.Models.Entities
{
    [BsonIgnoreExtraElements]
    public class RoleClaimPermissionMap
    {
        [BsonId]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string RoleId { get; set; } = string.Empty;

        public string ClaimPermissionId { get; set; } = string.Empty;
    }
}
