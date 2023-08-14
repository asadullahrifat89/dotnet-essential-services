using BaseCore.Extensions;
using BaseCore.Models.Entities;
using System.Text.Json.Serialization;
using TeamsCore.Declarations.Commands;

namespace TeamsCore.Models.Entities
{
    public class SearchCriteria : EntityBase
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string IconUrl { get; set; } = string.Empty;

        public SearchCriteriaType SearchCriteriaType { get; set; }

        public SkillsetType SkillsetType { get; set; }

        public static SearchCriteria Initialize(AddSearchCriteriaCommand command, AuthenticationContext authCtx)
        {
            var searchCriteria = new SearchCriteria()
            {
                Name = command.Name,
                Description = command.Description,
                IconUrl = command.IconUrl,
                SearchCriteriaType = command?.SearchCriteriaType ?? SearchCriteriaType.Discipline,
                SkillsetType = command?.SkillsetType ?? SkillsetType.Generic,
                TimeStamp = authCtx.BuildCreatedByTimeStamp(),
            };

            return searchCriteria;
        }

        public static SearchCriteria Initialize(SearchCriteria searchCriteria)
        {
            return new SearchCriteria()
            {
                Id = searchCriteria.Id,
                Name = searchCriteria.Name,
                Description = searchCriteria.Description,
                IconUrl = searchCriteria.IconUrl,
                SearchCriteriaType = searchCriteria.SearchCriteriaType,
                SkillsetType = searchCriteria.SkillsetType,
                TimeStamp = searchCriteria.TimeStamp
            };
        }
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
