using BaseCore.Models.Entities;

namespace LingoCore.Models.Entities
{
    public class LingoResource : EntityBase
    {
        public string AppId { get; set; } = string.Empty;

        public string LanguageCode { get; set; } = string.Empty;

        public string ResourceKey { get; set; } = string.Empty;

        public string ResourceValue { get; set; } = string.Empty;
    }
}
