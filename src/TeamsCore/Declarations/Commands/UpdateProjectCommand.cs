using BaseCore.Models.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsCore.Declarations.Commands
{
    public class UpdateProjectCommand : IRequest<ServiceResponse>
    {
        public string ProjectId { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Link { get; set; } = string.Empty;

        public string IconUrl { get; set; } = string.Empty;

        public string[] LinkedProductIds { get; set; } = new string[0];
    }
}
