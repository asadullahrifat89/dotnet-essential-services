using BaseCore.Models.Responses;
using MediatR;
using TeamsCore.Models.Entities;

namespace TeamsCore.Declarations.Commands
{
    public class AddSearchCriteriaCommand : IRequest<ServiceResponse>
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string IconUrl { get; set; } = string.Empty;

        public SearchCriteriaType? SearchCriteriaType { get; set; }

        public SkillsetType? SkillsetType { get; set; }
    }
}
