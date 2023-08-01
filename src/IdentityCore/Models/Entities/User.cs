using IdentityCore.Contracts.Declarations.Commands;
using IdentityCore.Extensions;

namespace IdentityCore.Models.Entities
{
    public class User : EntityBase
    {
        public string FirstName { get; set; } = string.Empty;

        public string LastName { get; set; } = string.Empty;

        public string DisplayName { get; set; } = string.Empty;

        public string Password { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string ProfileImageUrl { get; set; } = string.Empty;

        public string PhoneNumber { get; set; } = string.Empty;

        public static User Initialize(CreateUserCommand command)
        {
            var user = new User()
            {
                Email = command.Email,
                Password = command.Password.Encrypt(),
            };

            return user;
        }
    }
}
