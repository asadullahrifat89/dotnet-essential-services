namespace IdentityCore.Models.Entities
{
    public class User : EntityBase
    {
        public string Password { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;

        public string[] Claims { get; set; } = new string[0];
    }
}
