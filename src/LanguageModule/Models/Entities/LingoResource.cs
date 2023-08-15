using BaseModule.Domain.Entities;
using BaseModule.Infrastructure.Extensions;
using LanguageModule.Declarations.Commands;

namespace LanguageModule.Models.Entities
{
    public class LingoResource : EntityBase
    {
        public string AppId { get; set; } = string.Empty;

        public string LanguageCode { get; set; } = string.Empty;

        public string ResourceKey { get; set; } = string.Empty;

        public string ResourceValue { get; set; } = string.Empty;

        public static List<LingoResource> Initialize(AddLingoResourcesCommand command, AuthenticationContext authenticationContext)
        {
            var resourcesKeys = command.ResourceKeys.Select(x => x.ResourceKey).Distinct();
            var lingoResources = new List<LingoResource>();

            foreach (var resourceKey in resourcesKeys)
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

                    lingoResources.Add(lingoResource);
                }
            }

            return lingoResources;
        }
    }
}
