using IdentityCore.Contracts.Declarations.Commands;
using IdentityCore.Extensions;

namespace IdentityCore.Models.Entities
{
    public class User : EntityBase
    {
        public string Password { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

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
