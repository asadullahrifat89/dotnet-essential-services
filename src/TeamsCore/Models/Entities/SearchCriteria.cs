using BaseCore.Models.Entities;

namespace TeamsCore.Models.Entities
{
    public class SearchCriteria : EntityBase
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string IconUrl { get; set; } = string.Empty;

        public SearchCriteriaType SearchCriteriaType { get; set; }

        public SkillsetType SkillsetType { get; set; }
    }

    public enum SearchCriteriaType
    {
        Discipline,
        Skillset,
    }

    public enum SkillsetType
    {
        Generic,
        Hard,
        Soft,
    }
}
