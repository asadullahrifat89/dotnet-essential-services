﻿using MongoDB.Bson.Serialization.Attributes;

namespace Identity.Domain.Entities
{
    [BsonIgnoreExtraElements]
    public class RoleClaimPermissionMap
    {
        [BsonId]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string RoleId { get; set; } = string.Empty;

        public string ClaimPermission { get; set; } = string.Empty;
    }
}
