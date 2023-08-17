using Base.Domain.Entities;

namespace Identity.Domain.Entities
{
    public class AuthToken : EntityBase
    {
        public string AccessToken { get; set; } = string.Empty;

        public DateTime ExpiresOn { get; set; }

        public string RefreshToken { get; set; } = string.Empty;
    }
}
