using BaseModule.Domain.Entities;

namespace IdentityModule.Domain.Entities
{
    public class RefreshToken : BaseEntity
    {
        public string UserId { get; set; } = string.Empty;

        public string Jwt { get; set; } = string.Empty;
    }
}
