using BaseModule.Domain.Entities;

namespace IdentityModule.Domain.Entities
{
    public class ClaimPermission : EntityBase
    {
        public string Name { get; set; } = string.Empty;

        public string RequestUri { get; set; } = string.Empty;
    }
}
