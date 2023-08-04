
namespace BaseCommon
{
    public static class EndpointRoutes
    {
        public const string Action_GetUser = "api/Query/GetUser";
        public const string Action_GetUsers = "api/Query/GetUsers";
        public const string Action_CreateUser = "api/Command/CreateUser";
        public const string Action_UpdateUser = "api/Command/UpdateUser";
        public const string Action_UpdateUserPassword = "api/Command/UpdateUserPassword";
        public const string Action_UpdateUserRoles = "api/Command/UpdateUserRoles";

        public const string Action_AddRole = "api/Command/AddRole";
        public const string Action_UpdateRole = "api/Command/UpdateRole";

        public const string Action_AddClaimPermission = "api/Command/AddClaimPermission";

        public const string Action_ValidateToken = "api/Command/ValidateToken";
        public const string Action_Authenticate = "api/Command/Authenticate";

        public const string Action_GetEndPoints = "api/Query/GetEndpoints";

        public const string Action_GetRoles = "api/Query/GetRoles";
        public const string Action_GetUserRoles = "api/Query/GetUserRoles";

        public const string Action_GetClaims = "api/Query/GetClaims";

        
    }
}
