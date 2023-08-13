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
    public class AddProductCommand : IRequest<ServiceResponse>
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ManPower { get; set; } = string.Empty;

        public string Experience { get; set; } = string.Empty;

        public string EmploymentType { get; set; } = string.Empty;

        public ProductCostType ProductCostType { get; set; }

        public string IconUrl { get; set; } = string.Empty;

        public string BannerUrl { get; set; } = string.Empty;

        public string[] LinkedSearchCriteriaIds { get; set; } = new string[0];

        public string[] LinkedProjectIds { get; set; } = new string[0];
    }
}
