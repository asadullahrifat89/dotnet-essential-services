using BaseModule.Domain.Entities;
using IdentityModule.Domain.Entities;
using IdentityModule.Infrastructure.Extensions;
using LanguageModule.Application.Commands;

namespace LanguageModule.Domain.Entities
{
    public class LanguageResource : BaseEntity
    {
        public string AppId { get; set; } = string.Empty;

        public string LanguageCode { get; set; } = string.Empty;

        public string ResourceKey { get; set; } = string.Empty;

        public string ResourceValue { get; set; } = string.Empty;
    }
}
