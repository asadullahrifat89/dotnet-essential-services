using BaseCore.Models.Responses;
using MediatR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsCore.Models.Entities;

namespace TeamsCore.Declarations.Commands
{
    public class UpdateSearchCriteriaCommand : IRequest<ServiceResponse>
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string IconUrl { get; set; } = string.Empty;

        public SearchCriteriaType? SearchCriteriaType { get; set; }

        public SkillsetType? SkillsetType { get; set;}
    }
}
