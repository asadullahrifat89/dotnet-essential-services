namespace IdentityCore.Models.Entities
{
    public class AuthenticationContext
    {
        public string? RequestUri { get; set; }

        public User? User { get; set; } = null;

        public string? AccessToken { get; set; }

        public AuthenticationContext()
        {
            
        }

        public AuthenticationContext(string? rquestUri, User? user, string? accessToken)
        {
            RequestUri = rquestUri;
            User = user;
            AccessToken = accessToken;
        }
    }
}
