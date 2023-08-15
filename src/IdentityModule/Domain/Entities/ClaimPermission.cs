using BaseModule.Domain.Entities;
using IdentityModule.Application.Commands;
using IdentityModule.Infrastructure.Extensions;

namespace IdentityModule.Domain.Entities
{
    public class ClaimPermission : EntityBase
    {
        public string Name { get; set; } = string.Empty;

        public string RequestUri { get; set; } = string.Empty;
    }
}
