namespace IdentityCore.Models.Entities
{
    public class TimeStamp
    {
        public DateTime CreatedOn { get; set; } = DateTime.UtcNow;

        public DateTime? ModifiedOn { get; set; } = null;

        public string CreatedBy { get; set; } = string.Empty;

        public string ModifiedBy { get; set; } = string.Empty;
    }
}
