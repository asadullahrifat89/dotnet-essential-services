namespace BaseModule.Models.Entities
{
    public class AuthenticationContext
    {
        public string? RequestUri { get; set; }

        public UserBase? User { get; set; } = null;

        public string? AccessToken { get; set; }

        //public string TenantId { get; set; }

        public AuthenticationContext()
        {

        }

        public AuthenticationContext(string? rquestUri, UserBase? user, string? accessToken)
        {
            RequestUri = rquestUri;
            User = user;
            AccessToken = accessToken;
            //TenantId = user?.TenantId;
        }
    }
}
