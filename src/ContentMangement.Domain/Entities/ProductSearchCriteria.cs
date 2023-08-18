using Base.Domain.Entities;
using System.Text.Json.Serialization;

namespace Teams.ContentMangement.Domain.Entities
{
    public class ProductSearchCriteria : EntityBase
    {
        /// <summary>
        /// Name of the product search criteria.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Detailed description of the product search criteria.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Icon url of the product search criteria.
        /// </summary>
        public string IconUrl { get; set; } = string.Empty;

        ///// <summary>
        ///// The search criteria type of the product search criteria.
        ///// </summary>
        //public SearchCriteriaType SearchCriteriaType { get; set; }

        /// <summary>
        /// Based on the search criteria type, the skill set type of the the product search criteria.
        /// </summary>
        public SkillsetType SkillsetType { get; set; }
    }    

    //[JsonConverter(typeof(JsonStringEnumConverter))]
    //public enum SearchCriteriaType
    //{
    //    Discipline,
    //    Skillset,
    //}

    [JsonConverter(typeof(JsonStringEnumConverter))]
    public enum SkillsetType
    {
        Generic,
        Hard,
        Soft,
    }
}
