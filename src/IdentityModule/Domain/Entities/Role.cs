using BaseModule.Domain.Entities;

namespace IdentityModule.Domain.Entities
{
    public class Role : EntityBase
    {
        public string Name { get; set; } = string.Empty;
    }
}
