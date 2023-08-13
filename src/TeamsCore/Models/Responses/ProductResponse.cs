using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TeamsCore.Models.Entities;

namespace TeamsCore.Models.Responses
{
    public class ProductResponse : Product
    {
        public AttachedSearchCriteria[] AttachedSearchCriterias { get; set; } = new AttachedSearchCriteria[0];

        public AttachedProject[] AttachedProjects { get; set; } = new AttachedProject[0];
    }

    public class AttachedSearchCriteria
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public string IconUrl { get; set; } = string.Empty;
    }

    public class AttachedProject : AttachedSearchCriteria
    {
        public string Description { get; set; } = string.Empty;

        public string Link { get; set; } = string.Empty;        
    }
}
