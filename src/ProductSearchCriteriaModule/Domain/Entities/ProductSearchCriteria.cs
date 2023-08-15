using BaseModule.Domain.Entities;
using System.Text.Json.Serialization;

namespace SearchCriteriaModule.Domain.Entities
{
    public class ProductSearchCriteria : EntityBase
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string IconUrl { get; set; } = string.Empty;

        public ProductSearchCriteriaType SearchCriteriaType { get; set; }

        public SkillsetType SkillsetType { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum ProductSearchCriteriaType
    {
        Discipline,
        Skillset,
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SkillsetType
    {
        Generic,
        Hard,
        Soft,
    }
}
