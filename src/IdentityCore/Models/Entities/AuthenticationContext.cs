using IdentityCore.Models.Entities;

namespace IdentityCore.Models.Entities
{
    public class AuthenticationContext
    {
        public string? RequestUri { get; set; }

        public User? User { get; set; } = null;

        public AuthenticationContext()
        {
            
        }

        public AuthenticationContext(string? rquestUri, User? user)
        {
            RequestUri = rquestUri;
            User = user;
        }
    }
}
