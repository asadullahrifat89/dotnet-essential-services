
namespace BaseCommon
{
    public static class EndpointRoutes
    {
        public const string Action_GetUser = "api/Query/GetUser";
        public const string Action_GetUsers = "api/Query/GetUsers";
        public const string Action_CreateUser = "api/Command/CreateUser";
        public const string Action_SubmitUser = "api/Command/SubmitUser";
        public const string Action_UpdateUser = "api/Command/UpdateUser";
        public const string Action_UpdateUserPassword = "api/Command/UpdateUserPassword";
        public const string Action_UpdateUserRoles = "api/Command/UpdateUserRoles";

        public const string Action_AddRole = "api/Command/AddRole";
        public const string Action_UpdateRole = "api/Command/UpdateRole";

        public const string Action_ValidateToken = "api/Command/ValidateToken";
        public const string Action_AuthenticateToken = "api/Command/AuthenticateToken";

        public const string Action_GetEndPoints = "api/Query/GetEndpoints";

        public const string Action_GetRoles = "api/Query/GetRoles";
        public const string Action_GetUserRoles = "api/Query/GetUserRoles";

        public const string Action_GetClaims = "api/Query/GetClaims";
        public const string Action_AddClaimPermission = "api/Command/AddClaimPermission";

        public const string Action_UploadFile = "api/Command/UploadFile";
        public const string Action_DownloadFile = "api/Command/DownloadFile";
        public const string Action_GetFile = "api/Query/GetFile";

        public const string Action_CreateEmailTemplate = "api/Command/CreateEmailTemplate";
        public const string Action_GetEmailTemplate = "api/Query/GetEmailTemplate";
        public const string Action_UpdateEmailTemplate = "api/Command/UpdateEmailTemplate";

        public const string Action_EnqueueEmailMessage = "api/Command/EnqueueEmailMessage";

        public const string Action_AddLingoApp = "api/Command/AddLingoApp";
        public const string Action_AddLingoResources = "api/Command/AddLingoResources";
        public const string Action_GetLingoApp = "api/Query/GetLingoApp";
        public const string Action_GetLingoResourcesInFormat = "api/Query/GetLingoResourcesInFormat";

        public const string Action_SendUserAccountActivationRequest = "api/Command/SendUserAccountActivationRequest";
        public const string Action_VerifyUserAccountActivationRequest = "api/Command/VerifyUserAccountActivationRequest";

        public const string Action_AddSearchCriteria = "api/Command/AddSearchCriteria";
    }
}
