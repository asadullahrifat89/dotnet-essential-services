using BaseModule.Domain.Entities;

namespace BaseModule.Domain.DTOs.Responses
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

        public Address Address { get; set; } = new Address();

        public UserStatus UserStatus { get; set; }

        public static UserResponse Initialize(UserBase user)
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
                Address = user.Address,
                UserStatus = user.UserStatus,
            };
        }
    }
}
