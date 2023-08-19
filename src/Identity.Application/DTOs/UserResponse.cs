using Base.Domain.Entities;
using Identity.Domain.Entities;

namespace Identity.Application.DTOs
{
    public class UserResponse
    {
        public string Id { get; set; } = string.Empty;

        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string ProfileImageUrl { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public Address[] Addresses { get; set; } = new Address[] { };

        public UserStatus UserStatus { get; set; }

        public static UserResponse Map(UserBase user)
        {
            return new UserResponse()
            {
                Id = user.Id,
                FirstName = user.FirstName,
                LastName = user.LastName,
                DisplayName = user.DisplayName,
                Email = user.Email,
                ProfileImageUrl = user.ProfileImageUrl,
                PhoneNumber = user.PhoneNumber,
                Addresses = user.Addresses,
                UserStatus = user.UserStatus,
            };
        }
    }
}
