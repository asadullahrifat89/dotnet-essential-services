namespace Base.Shared.Constants
{
    public static class EndpointRoutes
    {
        public const string Action_Ping = "Query/Ping";

        public const string Action_GetUser = "Query/GetUser";
        public const string Action_GetUsers = "Query/GetUsers";
        public const string Action_CreateUser = "Command/CreateUser";
        public const string Action_SubmitUser = "Command/SubmitUser";
        public const string Action_UpdateUser = "Command/UpdateUser";
        public const string Action_UpdateUserPassword = "Command/UpdateUserPassword";
        public const string Action_UpdateUserRoles = "Command/UpdateUserRoles";

        public const string Action_AddRole = "Command/AddRole";
        public const string Action_UpdateRole = "Command/UpdateRole";

        public const string Action_ValidateToken = "Command/ValidateToken";
        public const string Action_AuthenticateToken = "Command/AuthenticateToken";

        public const string Action_GetEndPoints = "Query/GetEndpoints";

        public const string Action_GetRoles = "Query/GetRoles";
        public const string Action_GetUserRoles = "Query/GetUserRoles";

        public const string Action_GetClaims = "Query/GetClaims";
        public const string Action_AddClaimPermission = "Command/AddClaimPermission";

        public const string Action_UploadFile = "Command/UploadFile";
        public const string Action_DownloadFile = "Query/DownloadFile";
        public const string Action_GetFile = "Query/GetFile";

        public const string Action_CreateEmailTemplate = "Command/CreateEmailTemplate";
        public const string Action_GetEmailTemplate = "Query/GetEmailTemplate";
        public const string Action_GetEmailTemplateByPurpose = "Query/GetEmailTemplateByPurpose";
        public const string Action_UpdateEmailTemplate = "Command/UpdateEmailTemplate";

        public const string Action_EnqueueEmailMessage = "Command/EnqueueEmailMessage";

        public const string Action_AddLanguageApp = "Command/AddLanguageApp";
        public const string Action_AddLanguageResources = "Command/AddLanguageResources";
        public const string Action_GetLanguageApp = "Query/GetLanguageApp";
        public const string Action_GetLanguageResourcesInFormat = "Query/GetLanguageResourcesInFormat";

        public const string Action_SendUserAccountActivationRequest = "Command/SendUserAccountActivationRequest";
        public const string Action_VerifyUserAccountActivationRequest = "Command/VerifyUserAccountActivationRequest";       
    }
}
