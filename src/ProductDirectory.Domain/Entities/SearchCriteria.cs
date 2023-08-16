using BaseModule.Domain.Entities;
using System.Text.Json.Serialization;

namespace ProductDirectory.Domain.Entities
{
    public class SearchCriteria : EntityBase
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string IconUrl { get; set; } = string.Empty;

        public SearchCriteriaType SearchCriteriaType { get; set; }

        public SkillsetType SkillsetType { get; set; }
    }

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SearchCriteriaType
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
