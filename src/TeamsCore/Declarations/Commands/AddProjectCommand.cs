using BaseCore.Models.Responses;
using MediatR;

namespace TeamsCore.Declarations.Commands
{
    public class AddProjectCommand : IRequest<ServiceResponse>
    { 
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string Link { get; set; } = string.Empty;

        public string IconUrl { get; set; } = string.Empty;

        public string[] LinkedProductIds { get; set; } = new string[0]; 
    }
}
