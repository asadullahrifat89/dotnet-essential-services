using BaseModule.Domain.Entities;

namespace IdentityModule.Domain.Entities
{
    public class AuthToken : BaseEntity
    {
        public string AccessToken { get; set; } = string.Empty;

        public DateTime ExpiresOn { get; set; }

        public string RefreshToken { get; set; } = string.Empty;
    }
}
