using Base.Domain.Entities;

namespace Identity.Domain.Entities
{
    public class Role : EntityBase
    {
        public string Name { get; set; } = string.Empty;
    }
}
