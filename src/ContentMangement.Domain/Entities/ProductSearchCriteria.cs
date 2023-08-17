using Base.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace ContentMangement.Domain.Entities
{
    public class ProductSearchCriteria : EntityBase
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
