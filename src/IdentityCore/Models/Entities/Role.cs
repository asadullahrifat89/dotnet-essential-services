using IdentityCore.Contracts.Declarations.Commands;
using IdentityCore.Extensions;
using MongoDB.Bson.Serialization.Attributes;

namespace IdentityCore.Models.Entities
{
    [BsonIgnoreExtraElements]
    public class Role : EntityBase
    {
        public string Name { get; set; } = string.Empty;

        public static Role Initialize(AddRoleCommand command, AuthenticationContext authenticationContext)
        {
            return new Role()
            {
                Name = command.Name,
                TimeStamp = authenticationContext.BuildCreatedByTimeStamp(),
            };
        }
    }
}
