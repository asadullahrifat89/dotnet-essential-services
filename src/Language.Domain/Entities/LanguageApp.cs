using Base.Domain.Entities;

namespace Language.Domain.Entities
{
    public class LanguageApp : EntityBase
    {
        public string Name { get; set; } = string.Empty;
    }
}
