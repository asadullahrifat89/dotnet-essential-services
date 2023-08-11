using BaseCore.Models.Entities;
using BaseCore.Extensions;
using LingoCore.Declarations.Commands;

namespace LingoCore.Models.Entities
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
