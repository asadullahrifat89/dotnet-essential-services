using BaseCore.Extensions;
using BaseCore.Models.Entities;
using LingoCore.Declarations.Commands;

namespace LingoCore.Models.Entities
{
    public class LingoResource : EntityBase
    {
        public string AppId { get; set; } = string.Empty;

        public string LanguageCode { get; set; } = string.Empty;

        public string ResourceKey { get; set; } = string.Empty;

        public string ResourceValue { get; set; } = string.Empty;


        public static LingoResource Initialize(AddLingoResourcesCommand command, AuthenticationContext authenticationContext)
        {
            var resourcesKey = command.ResourceKeys.Select(x => x.ResourceKey).Distinct();

            foreach (var resourceKey in resourcesKey)
            {
                var foundResourceKey = command.ResourceKeys.FirstOrDefault(x => x.ResourceKey.Equals(resourceKey));

                foreach (var cultureValue in foundResourceKey.CultureValues)
                {
                    var lingoResource = new LingoResource()
                    {
                        AppId = command.AppId,
                        LanguageCode = cultureValue.LanguageCode,
                        ResourceKey = resourceKey,
                        ResourceValue = cultureValue.ResourceValue,
                        TimeStamp = authenticationContext.BuildCreatedByTimeStamp(),
                    };

                    return lingoResource;
                }
            }

            return null;
        }
    }
}
