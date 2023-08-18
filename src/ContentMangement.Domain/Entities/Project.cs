using Base.Domain.Entities;

namespace Teams.ContentMangement.Domain.Entities
{
    public class Project : EntityBase
    {
        /// <summary>
        /// Name of the project.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Detailed description of the project.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Icon image url of this project. It can be the company image for which this project is developed.
        /// </summary>
        public string IconUrl { get; set; } = string.Empty;

        /// <summary>
        /// Browsable link to the project.
        /// </summary>
        public string ProjectLink { get; set; } = string.Empty;

        /// <summary>
        /// Link to the client's website.
        /// </summary>
        public string ClientLink { get; set; } = string.Empty;       

        /// <summary>
        /// Array or image urls. It can contain screenshots of the project pages.
        /// </summary>
        public string[] ImageUrls { get; set; } = new string[] { };
    }
}
