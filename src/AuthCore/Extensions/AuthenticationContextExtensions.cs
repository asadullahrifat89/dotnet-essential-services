using BaseCore.Models.Entities;

namespace BaseCore.Extensions
{
    public static class AuthenticationContextExtensions
    {
        public static TimeStamp BuildCreatedByTimeStamp(this AuthenticationContext authenticationContext)
        {
            return new TimeStamp() { CreatedBy = authenticationContext.User?.Id };
        }

        //public static TimeStamp BuildModifiedByTimeStamp(this AuthenticationContext authenticationContext, TimeStamp timeStamp)
        //{
        //    return new TimeStamp()
        //    {
        //        CreatedBy = timeStamp.CreatedBy,
        //        CreatedOn = timeStamp.CreatedOn,
        //        ModifiedBy = authenticationContext.User?.Id,
        //        ModifiedOn = DateTime.UtcNow
        //    };
        //}
    }
}
