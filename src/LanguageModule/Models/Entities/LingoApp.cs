using BaseModule.Domain.Entities;
using BaseModule.Extensions;
using LanguageModule.Declarations.Commands;

namespace LanguageModule.Models.Entities
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
