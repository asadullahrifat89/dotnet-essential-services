using Base.Domain.Entities;

namespace LanguageModule.Domain.Entities
{
    public class LanguageApp : EntityBase
    {
        public string Name { get; set; } = string.Empty;
    }
}
