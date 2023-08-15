using BaseModule.Domain.Entities;
using IdentityModule.Domain.Entities;
using IdentityModule.Infrastructure.Extensions;
using LanguageModule.Application.Commands;

namespace LanguageModule.Domain.Entities
{
    public class LanguageApp : BaseEntity
    {
        public string Name { get; set; } = string.Empty;
    }
}
