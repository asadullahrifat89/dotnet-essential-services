using Base.Application.DTOs.Responses;
using Identity.Application.Extensions;
using Identity.Domain.Entities;
using MediatR;
using Teams.ContentMangement.Domain.Entities;

namespace Teams.ContentMangement.Application.Commands
{
    public class AddProjectCommand : IRequest<ServiceResponse>
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string IconUrl { get; set; } = string.Empty;

        public string ProjectLink { get; set; } = string.Empty;

        public string ClientLink { get; set; } = string.Empty;

        public string[] ImageUrls { get; set; } = new string[] { };

        public string[] LinkedProductIds { get; set; } = new string[] { };

        public static Project Initialize(AddProjectCommand command, AuthenticationContext authenticationContext)
        {
            var project = new Project()
            {
                Name = command.Name,
                Description = command.Description,
                IconUrl = command.IconUrl,
                ProjectLink = command.ProjectLink,
                ClientLink = command.ClientLink,
                ImageUrls = command.ImageUrls,
                PublishingStatus = PublishingStatus.Published,
                TimeStamp = authenticationContext.BuildCreatedByTimeStamp(),
            };

            return project;
        }
    }
}
