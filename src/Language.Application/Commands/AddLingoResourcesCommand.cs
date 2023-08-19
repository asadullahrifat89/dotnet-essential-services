using Base.Application.DTOs.Responses;
using Identity.Application.Extensions;
using Identity.Domain.Entities;
using Language.Domain.Entities;
using MediatR;

namespace Language.Application.Commands
{
    public class AddLingoResourcesCommand : IRequest<ServiceResponse>
    {
        public string AppId { get; set; } = string.Empty;

        public List<ResourceKeyEntry> ResourceKeys { get; set; } = new List<ResourceKeyEntry>();

        public class ResourceKeyEntry
        {
            public string ResourceKey { get; set; } = string.Empty;
            public List<CultureValue> CultureValues { get; set; } = new List<CultureValue>();
        }

        public class CultureValue
        {
            public string LanguageCode { get; set; } = string.Empty;
            public string ResourceValue { get; set; } = string.Empty;
        }

        public static List<LanguageResource> Map(AddLingoResourcesCommand command, AuthenticationContext authenticationContext)
        {
            var resourcesKeys = command.ResourceKeys.Select(x => x.ResourceKey).Distinct();
            var lingoResources = new List<LanguageResource>();

            foreach (var resourceKey in resourcesKeys)
            {
                var foundResourceKey = command.ResourceKeys.FirstOrDefault(x => x.ResourceKey.Equals(resourceKey));

                foreach (var cultureValue in foundResourceKey.CultureValues)
                {
                    var lingoResource = new LanguageResource()
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
