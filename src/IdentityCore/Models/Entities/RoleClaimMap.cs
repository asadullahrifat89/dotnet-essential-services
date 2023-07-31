using MongoDB.Bson.Serialization.Attributes;

namespace IdentityCore.Models.Entities
{
    [BsonIgnoreExtraElements]
    public class RoleClaimMap
    {
        [BsonId]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string RoleId { get; set; } = string.Empty;

        public string ClaimId { get; set; } = string.Empty;
    }
}
