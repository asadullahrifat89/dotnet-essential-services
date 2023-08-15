using BaseModule.Domain.Entities;
using IdentityModule.Domain.Entities;
using IdentityModule.Infrastructure.Extensions;
using LanguageModule.Application.Commands;

namespace LanguageModule.Domain.Entities
{
    public class LingoApp : EntityBase
    {
        public string Name { get; set; } = string.Empty;

        public static LingoApp Initialize(AddLingoAppCommand command, AuthenticationContext authenticationContext)
        {
            var lingoApp = new LingoApp()
            {
                Name = command.Name,
                TimeStamp = authenticationContext.BuildCreatedByTimeStamp(),
            };

            return lingoApp;
        }
    }
}
