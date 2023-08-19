namespace Base.Shared.Constants
{
    public static class CommonConstants
    {
        public static string[] Client_Origins = new string[]
        {
            "https://*.seliselocal.com",
            "https://*.selise.biz",
        };

        public static string[] AllowedSwaggerEnvironments = new[] { "Development", "dev-az", };

        public static string Consumer_Tag = "consumer";
        public static string Provider_Tag = "provider";

        public static string[] OnboardUserMetaTags = new[] { Consumer_Tag, Provider_Tag };
    }
}
