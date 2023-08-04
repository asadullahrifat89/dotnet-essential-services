namespace BaseCore.Models.Entities
{
    public class RefreshToken : EntityBase
    {
        public string UserId { get; set; } = string.Empty;

        public string Jwt { get; set; } = string.Empty;
    }
}
