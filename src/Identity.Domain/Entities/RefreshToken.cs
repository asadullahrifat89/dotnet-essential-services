using Base.Domain.Entities;

namespace Identity.Domain.Entities
{
    public class RefreshToken : EntityBase
    {
        public string UserId { get; set; } = string.Empty;

        public string Jwt { get; set; } = string.Empty;
    }
}
