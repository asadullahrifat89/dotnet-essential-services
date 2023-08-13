using BaseCore.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TeamsCore.Models.Entities
{
    public class Product : EntityBase
    {
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;

        public string ManPower { get; set; } = string.Empty;

        public string Experience { get; set; } = string.Empty;

        public ProductCostType ProductCostType { get; set; }

        public string IconUrl { get; set; } = string.Empty;

        public string BannerUrl { get; set; } = string.Empty;
    }

    public enum ProductCostType
    {
        Low,
        Medium,
        High
    }
}
