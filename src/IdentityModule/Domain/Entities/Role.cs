using BaseModule.Domain.Entities;
using IdentityModule.Application.Commands;
using IdentityModule.Infrastructure.Extensions;

namespace IdentityModule.Domain.Entities
{
    public class Role : EntityBase
    {
        public string Name { get; set; } = string.Empty;
    }
}
