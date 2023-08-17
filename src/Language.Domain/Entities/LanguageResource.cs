using Base.Domain.Entities;

namespace Language.Domain.Entities
{
    public class LanguageResource : EntityBase
    {
        public string AppId { get; set; } = string.Empty;

        public string LanguageCode { get; set; } = string.Empty;

        public string ResourceKey { get; set; } = string.Empty;

        public string ResourceValue { get; set; } = string.Empty;
    }
}
