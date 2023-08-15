using BaseModule.Domain.Entities;
using System.Text.Json.Serialization;

namespace IdentityModule.Domain.Entities
{
    public class UserBase : BaseEntity
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string ProfileImageUrl { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public Address Address { get; set; } = new Address();

        public UserStatus UserStatus { get; set; } = UserStatus.Inactive;

        public string[] MetaTags { get; set; } = new string[] { };

    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum UserStatus
    {
        Active = 0,
        Inactive = 1,
    }
}
