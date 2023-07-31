using IdentityCore.Contracts.Declarations.Commands;
using MongoDB.Bson.Serialization.Attributes;

namespace IdentityCore.Models.Entities
{
    [BsonIgnoreExtraElements]
    public class Role
    {
        [BsonId]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        public string Name { get; set; } = string.Empty;

        public static Role Initialize(AddRoleCommand command)
        {
            return new Role()
            {
                Name = command.Name,                 
            };
        }
    }
}
