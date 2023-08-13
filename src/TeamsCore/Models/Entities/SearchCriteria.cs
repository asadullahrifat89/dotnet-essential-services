using BaseCore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsCore.Models.Entities
{
    public class SearchCriteria : EntityBase
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Icon { get; set; } = string.Empty;

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
