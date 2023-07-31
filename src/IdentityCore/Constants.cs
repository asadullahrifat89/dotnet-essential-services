using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IdentityCore
{
    public static class Constants
    {
        public static string[] Client_Origins = new string[]
        {
            "https://*.seliselocal.com",
        };

        public static string[] AllowedSwaggerEnvironments = new[] { "Development", "dev-az" };

        public static string[] Claims = new[] { "create-user", "read-user", "update-user", "delete-user" };
    }
}
