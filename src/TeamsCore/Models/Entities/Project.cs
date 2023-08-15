﻿using BaseCore.Models.Entities;
using BaseCore.Extensions;
using TeamsCore.Declarations.Commands;
using BaseCore.Models.Responses;

namespace TeamsCore.Models.Entities
{
    public class Project : EntityBase
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Link { get; set; } = string.Empty;

        public string IconUrl { get; set; } = string.Empty;

        public string[] LinkedProductIds { get; set; } = new string[0];

        public static Project Initialize(AddProjectCommand command, AuthenticationContext authenticationContext)
        {
            var project = new Project()
            {
                Name = command.Name,
                Description = command.Description,
                Link = command.Link,
                IconUrl = command.IconUrl,
                TimeStamp = authenticationContext.BuildCreatedByTimeStamp(),
            };

            return project;
        }
        public static Project Initialize(Project project)
        {
            return new Project()
            {
                Id = project.Id,
                Name = project.Name,
                Description = project.Description,
                Link = project.Link,
                IconUrl = project.IconUrl,
                LinkedProductIds = project.LinkedProductIds,
                TimeStamp = project.TimeStamp,
            };
        }
    }

   
}