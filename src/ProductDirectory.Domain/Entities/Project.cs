using BaseModule.Domain.Entities;

namespace ProductDirectory.Domain.Entities
{
    public class Project : EntityBase
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Link { get; set; } = string.Empty;

        public string IconUrl { get; set; } = string.Empty;

        public string[] LinkedProductIds { get; set; } = new string[0];
    }
}
