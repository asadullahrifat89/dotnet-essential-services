using Identity.Domain.Entities;
using Teams.ContentMangement.Domain.Entities;

namespace Teams.ContentMangement.Application.Commands
{
    public class UpdateProjectCommand : AddProjectCommand
    {
        public string ProjectId { get; set; } = string.Empty;

        public PublishingStatus PublishingStatus { get; set; } = PublishingStatus.Published;

        public static Project Initialize(UpdateProjectCommand command, AuthenticationContext authenticationContext)
        {
            var project = new Project()
            {
                Id = command.ProjectId,
                Name = command.Name,
                Description = command.Description,
                IconUrl = command.IconUrl,
                ProjectLink = command.ProjectLink,
                ClientLink = command.ClientLink,
                ImageUrls = command.ImageUrls,
                PublishingStatus = command.PublishingStatus,
            };

            return project;
        }
    }
}
